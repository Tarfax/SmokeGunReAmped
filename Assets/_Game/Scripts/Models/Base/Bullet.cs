using UnityEngine;

public abstract class Bullet : MonoBehaviour {

    protected new Transform transform;
    protected BulletData bulletData;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    protected Vector3 startingPosition;
    [SerializeField] private LayerMask layerMask;
    protected bool isColliding;

    [SerializeField] protected bool isActive;

    private void Start() {
        transform = base.transform;
        startingPosition = transform.position;
        isActive = true;
    }

    private void Update() {
        if (isActive == true) {
            float deltaTime = Time.deltaTime;
            currentPosition = transform.position;

            Move(deltaTime);
            CheckForCollision();
            CheckBulletDistance();

            previousPosition = currentPosition;
        }
    }

    protected abstract void Move(float deltaTime);
    protected abstract void OnCollision(RaycastHit hit);

    private void CheckBulletDistance() {
        float distanceFromStart = Vector3.Distance(startingPosition, transform.position);
        if (distanceFromStart > bulletData.BulletDistance) {
            isActive = false;
            Destroy(gameObject);
        }
    }

    public void SetBulletData(BulletData data) {
        bulletData = data;
        foreach (var item in data.GetLayerMask()) {
            layerMask += item;
        }
    }

    private void CheckForCollision() {
        RaycastHit hit;
        float rayCastDistance = Mathf.Clamp01(Vector3.Distance(currentPosition, previousPosition));
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayCastDistance, layerMask)) {
            OnCollision(hit);
            isActive = false;
            Destroy(gameObject);
        }
    }

    protected abstract HitData GetBulletHitData(RaycastHit hit, int bulletDamage);

    private void OnDrawGizmosSelected() {
        Transform transform = base.transform;
        float rayCastDistance = Vector3.Distance(currentPosition, previousPosition);
        Debug.DrawRay(transform.position, transform.forward * rayCastDistance);

        if (Physics.Raycast(transform.position, transform.forward, rayCastDistance)) {
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward * rayCastDistance, Color.green);
        }
    }

}
