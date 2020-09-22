using UnityEngine;

public class Rotator : MonoBehaviour {

    private new Transform transform;
    [Range(0f, 10f)] [SerializeField] private float rotationSpeed = 2f;

    void Start() {
        transform = base.transform;
    }

    void Update() {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime * -50f;
        transform.rotation = Quaternion.Euler(rotation);
    }

}
