using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    public static Camera Camera { get; private set; }

#pragma warning disable CS0649
    private Transform cameraTransform;
    [SerializeField] private Transform cameraMidPoint;
    public static Transform CameraMidPoint;
#pragma warning restore CS0649

    [SerializeField] private Transform cameraMiddleSpawnTrigger = default;

    public static Transform CameraLeftSpawnTrigger;
    public static Transform CameraMiddleSpawnTrigger;
    public static Transform CameraRightSpawnTrigger;
    public static Vector3 Position { get; private set; }

    [SerializeField] private float cameraSpeed = 1.75f;
    [SerializeField] private float speedSmoothness = 1f;
    [SerializeField] private float fov = 65f;
    [SerializeField] private float cameraHeight = 15f;
    [SerializeField] private float rotation = 63f;

    private float smoothVelocity;
    private float speed;

    public void Awake() {
        Camera = GetComponent<Camera>();
        CameraMiddleSpawnTrigger = cameraMiddleSpawnTrigger;

        CameraMidPoint = cameraMidPoint;
        cameraTransform = Camera.transform;
    }

    private void OnValidate() {
        Position = transform.position;
        Camera camera = GetComponent<Camera>();
        camera.fieldOfView = fov;
        transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(rotation, 0f, 0f));
    }

    private void LateUpdate() {
        Position = cameraTransform.position;

        speed = Mathf.SmoothDamp(speed, cameraSpeed, ref smoothVelocity, speedSmoothness);
        cameraTransform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
    }

}
