using MC_Utility;
using UnityEngine;

public class ItemDropCoordinator : MonoBehaviour {

    void Start() {
        EventSystem<DeathEvent>.RegisterListener(OnDeathEvent);
    }

    private void OnBarrelDeath(DeathEvent barrelDeathEvent) {
        LootData itemDropData = barrelDeathEvent.GetDrop();
        if (itemDropData == null) {
            return;
        }
        itemDropData.Initialize();
        SpawnDropItem(barrelDeathEvent.Position, itemDropData.Visual).GetComponent<Item>().SetDropItemData(itemDropData);
    }

    private void OnDeathEvent(DeathEvent deathEvent) {
        if (deathEvent.DeathType == DeathType.ByDistance) {
            return;
        }

        LootData itemDropData = deathEvent.GetDrop();
        if (itemDropData == null) {
            return;
        }
        SpawnDropItem(deathEvent.Position, itemDropData.Visual).GetComponent<Item>().SetDropItemData(itemDropData);
        itemDropData.Initialize();
    }

    private GameObject SpawnDropItem(Vector3 position, GameObject gameObject) {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 1f, Layers.Ground)) {
            position = hit.point;
        }
        return Instantiate(gameObject, position, transform.rotation);
    }

    private void OnDisable() {
        EventSystem<DeathEvent>.UnregisterListener(OnDeathEvent);
    }

}