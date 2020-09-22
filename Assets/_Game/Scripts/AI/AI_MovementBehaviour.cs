using System;
using UnityEngine;

[Serializable]
public class AI_MovementBehaviour {

    private Transform transform = default;
    private Animator animator = default;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.0f;

    [Header("Movement Style")]
    [SerializeField] private AI_MovementType movementType = default;
    [Space]
    [SerializeField] private MoveStraightForward straightForward = default;
    [SerializeField] private MoveInSineWaves sineWave = default;
    [SerializeField] private StandingStill standingStill = default;
    [SerializeField] private MoveSmart smart = default;
    [SerializeField] private MoveTowardsPlayer towardsPlayer = default;
    private IMovement movement = default;

    float velocityY;
    float gravity = -10f;
    float skinWidth = 0.1f;
    float raycastToGroundDistance = 0.5f;
    Vector3 raycastToGroundStartPosition = Vector3.up * 0.2f;

    public void Start(Transform transform, Animator anim) {
        this.transform = transform;
        animator = anim;
        SelectMovementStyle();
    }

    public void SelectMovementStyle() {
        switch (movementType) {
            case AI_MovementType.SineWave:
                movement = sineWave;
                break;
            case AI_MovementType.StandingStill:
                movement = standingStill;
                break;
            case AI_MovementType.StraightForward:
                movement = straightForward;
                break;
            case AI_MovementType.Smart:
                movement = smart;
                break;
            case AI_MovementType.TowardsPlayer:
                movement = towardsPlayer;
                break;

        }
        if (transform != null)
            movement.Initialize(transform, animator);
        movement.SetMoveSpeed(moveSpeed);
    }

    public void Update(float deltaTime) {
        movement.Update(deltaTime);

        RaycastHit hit;
        velocityY = gravity * deltaTime;
        LayerMask layerMask = Layers.Ground & Layers.BulletCollider;
        if (Physics.Raycast(transform.position + raycastToGroundStartPosition, Vector3.down, out hit, raycastToGroundDistance, Layers.Ground) == false) {
            transform.position = new Vector3(transform.position.x, transform.position.y + velocityY, transform.position.z);
        }
        else {
            transform.position = new Vector3(transform.position.x, hit.point.y + skinWidth, transform.position.z);
            velocityY = 0f;
        }
    }

    public void LateUpdate() {
        movement.LateUpdate();
    }

    public void OnValidate() {
        if (movement != null && movementType != movement.MovementType) {
            SelectMovementStyle();
        }
    }

    public void OnDrawGizmo(Transform transform) {
        smart.OnDrawGizmo();
    }

}
