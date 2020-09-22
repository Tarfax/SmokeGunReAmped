using MC_Utility;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public static float DistanceWalked;
    private static int instances;

    private CharacterController characterController;

    [HideInInspector] [SerializeField] private static Vector3 position;
    public static Vector3 Position => position;

    private static Vector3 forward;
    public static Vector3 Forward => forward;

    private static Quaternion rotation;
    public static Quaternion Rotation => rotation;

    public static GameObject GameObject { get; private set; }

    private bool isAlive = true;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    [Header("Movement")]
    [SerializeField] private Animator animator = default;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float diagonalSpeedModifier = 0.75f;
    private new Transform transform; // Caching the transform

    [SerializeField] private float gravity = -12;
    private float velocityY;
    bool isCameraColliding = false;


#pragma warning disable 0414
    private bool isGrounded = false; //For Debug
#pragma warning restore 0414

    [Header("Shooting")]
    [SerializeField] bool useClassicAimController = true;
    [SerializeField] private float maxShootingAngle = 37f;
    private float shootingAngle;
    private float targetShootingAngle;
    [SerializeField] private float shootingAngleSmoothness = 2f;

    // If not classic aim controller
    private float switchAngleTime = 0.12f;
    private float switchAngleTimer;
    private bool useAngle = false;

#pragma warning disable 0414
    [Header("Raycasting")]
    [SerializeField] private bool useDebugGizmos = false;
    [SerializeField] private float raycastToGroundDistance = 0.5f;
    [SerializeField] private float skinWidth = 0.1f;
#pragma warning restore 0414

    private Action onFirePressed;
    private Action onHomingMissilePressed;
    private Action onForceFieldPressed;
    private Action onForceFieldReleased;
    private Action onChangeWeapon;
    private void Start() {
        GameObject = gameObject;
        characterController = GetComponent<CharacterController>();

        transform = base.transform;
        switchAngleTimer = switchAngleTime;
        isAlive = true;
        DistanceWalked = 0f;
        EventSystem<DeathEvent_Player>.RegisterListener(OnDead);
    }

    private void OnDead(DeathEvent_Player deathEvent) {
        isAlive = false;
    }

    private void Update() {
        if (isAlive == true) {


            position = transform.position;
            forward = transform.forward;
            rotation = transform.rotation;

            float horizontal = Input.GetAxisRaw(Horizontal);
            float vertical = Input.GetAxisRaw(Vertical);
            Vector2 input = new Vector2(horizontal, vertical);

            Move(input);

            if (useClassicAimController == true) {
                ClassicAim();
            }
            else {
                AimAngleMovement(input);
            }

            if (Input.GetKey(KeyCode.LeftShift) == true) {
                if (onForceFieldPressed != null)
                    onForceFieldPressed();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) == true) {
                if (onForceFieldReleased != null)
                    onForceFieldReleased();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
                if (onHomingMissilePressed != null) {
                    onHomingMissilePressed();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab) == true) {
                if (onChangeWeapon != null) {
                    onChangeWeapon();
                }
            }
        }

    }

    private void Move(Vector2 input) {
        float deltaTime = Time.deltaTime;
        Vector3 position = transform.position;

        float speed = moveSpeed;
        if (Mathf.Abs(input.x) > 0f && Mathf.Abs(input.y) > 0f) {
            speed = moveSpeed * diagonalSpeedModifier;
        }

        Vector3 direction = new Vector3(input.x, 0f, input.y);
        Vector3 deltaDirection = direction * speed * deltaTime;

        velocityY += deltaTime * gravity;

        characterController.Move(deltaDirection + Vector3.up * velocityY);

        Vector3 newPosition = AdjustForCameraCollision(position, deltaDirection);
        if (isCameraColliding == true) {
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }

        if (characterController.isGrounded == true) {
            velocityY = 0f;
        }

        animator.SetFloat("VelocityX", direction.x, 0.02f, deltaTime);
        animator.SetFloat("VelocityY", direction.z, 0.02f, deltaTime);

        DistanceWalked += Vector3.Distance(transform.position, position);
    }

    private void ClassicAim() {
        bool shouldFire = false;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            targetShootingAngle = -maxShootingAngle;
            shouldFire = true;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            targetShootingAngle = maxShootingAngle;
            shouldFire = true;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            targetShootingAngle = 0f;
            shouldFire = true;
        }

        shootingAngle = Mathf.MoveTowards(shootingAngle, targetShootingAngle, shootingAngleSmoothness);
        transform.eulerAngles = Vector3.up * shootingAngle;

        if (shouldFire == true && onFirePressed != null) {
            onFirePressed();
        }
    }

    private void AimAngleMovement(Vector2 input) {
        Vector3 inputDirection = input.normalized;

        if (Mathf.Abs(inputDirection.x) > 0f) {
            if (Mathf.Abs(inputDirection.y) > 0f) {
                useAngle = true;
            }
            switchAngleTimer = switchAngleTime;
        }
        else {
            if (useAngle == true) {
                switchAngleTimer -= Time.deltaTime;
                if (switchAngleTimer < 0f) {
                    useAngle = false;
                    switchAngleTimer = switchAngleTime;
                }
            }
        }

        if (inputDirection.y < 0f) {
            inputDirection.y = 0f;
        }

        float angle = Mathf.Atan2(inputDirection.x, inputDirection.y) * (Mathf.Rad2Deg);

        if (Mathf.Abs(angle) > maxShootingAngle) {
            if (Mathf.Abs(input.x) > 0f || useAngle == true) {
                angle = angle < 0 ? -maxShootingAngle : maxShootingAngle;
            }
            else {
                angle = 0f;
            }
        }

        transform.eulerAngles = Vector3.up * angle;
    }

    private Vector3 AdjustForCameraCollision(Vector3 position, Vector3 direction) {
        isCameraColliding = false;

        Vector3 positionToCheck = position + (direction * 1.02f);

        Vector3 worldToViewPort = CameraController.Camera.WorldToViewportPoint(positionToCheck);

        worldToViewPort.x = Mathf.Clamp01(worldToViewPort.x);
        worldToViewPort.y = Mathf.Clamp01(worldToViewPort.y);

        if (IsCollidingWithCamera(ref worldToViewPort.x, false) == true) {
            isCameraColliding = true;
        }

        if (IsCollidingWithCamera(ref worldToViewPort.y) == true) {
            isCameraColliding = true;
        }

        if (isCameraColliding == false) {
            return position + direction;
        }

        worldToViewPort = CameraController.Camera.ViewportToWorldPoint(worldToViewPort);
        return worldToViewPort;
    }

    private bool IsCollidingWithCamera(ref float viewPortDistance, bool checkLower = true) {
        if (viewPortDistance > 0.9f) {
            viewPortDistance = 0.9f;
            return true;
        }
        else if (viewPortDistance < 0.1f && checkLower == true) {
            viewPortDistance = 0.1f;
            return true;
        }
        return false;
    }

    // All these callbacks I usualy extract to an input method that handles them separately \\
    public void RegisterOnFirePressedCallback(Action callbackFunc) {
        onFirePressed += callbackFunc;
    }

    public void UnregisterOnFirePressedCallback(Action callbackFunc) {
        onFirePressed -= callbackFunc;
    }

    public void RegisterOnFireHomingMissilePressedCallback(Action callbackFunc) {
        onHomingMissilePressed += callbackFunc;
    }

    public void UnregisterOnFireHomingMissilePressedCallback(Action callbackFunc) {
        onHomingMissilePressed -= callbackFunc;
    }

    public void RegisterOnForceFieldPressedCallback(Action callbackFunc) {
        onForceFieldPressed += callbackFunc;
    }

    public void UnregisterOnForceFieldPressedCallback(Action callbackFunc) {
        onForceFieldPressed -= callbackFunc;
    }

    public void RegisterOnForceFieldReleasedCallback(Action callbackFunc) {
        onForceFieldReleased += callbackFunc;
    }

    public void UnregisterOnForceFieldReleasedCallback(Action callbackFunc) {
        onForceFieldReleased -= callbackFunc;
    }

    public void RegisterOnChangeWeaponCallback(Action callbackFunc) {
        onChangeWeapon += callbackFunc;
    }

    public void UnregisterOnChangeWeaponCallback(Action callbackFunc) {
        onChangeWeapon -= callbackFunc;
    }

    private void OnDrawGizmos() {
        position = base.transform.position;
    }

    private void OnDisable() {
        EventSystem<DeathEvent_Player>.UnregisterListener(OnDead);
    }

}
