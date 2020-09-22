using UnityEngine;

public abstract class Item : MonoBehaviour {
    private new Transform transform;
    private bool isActive;
    private float collectRadius;

    private void Start() {
        transform = base.transform;
        isActive = true;
        collectRadius = GetComponent<SphereCollider>().radius * 2;
    }

    public void Update() {
        if (isActive == true) {
            if (Vector3.Distance(transform.position, PlayerController.Position) < collectRadius) {
                OnPickupItem();
                isActive = false;
            }
            else if (Vector3.Distance(transform.position, PlayerController.Position) > 50f) {
                OnDistanceTooFar();
                isActive = false;
            }
        }
    }

    protected abstract void OnPickupItem();

    protected abstract void OnDistanceTooFar();

    public abstract void SetDropItemData(LootData data);

}
