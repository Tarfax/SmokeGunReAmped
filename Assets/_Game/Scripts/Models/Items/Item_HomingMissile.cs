using MC_Utility;
using UnityEngine;

public class Item_HomingMissile : Item {

    [SerializeField] private LootData_HomingMissile itemDropData;

    public override void SetDropItemData(LootData data) {
        itemDropData = data as LootData_HomingMissile;
    }

    protected override void OnDistanceTooFar() {
        Destroy(gameObject);
    }

    protected override void OnPickupItem() {
        EventSystem<PickupLootEvent>.FireEvent<LootData_HomingMissile>(GetEventData());
        Destroy(gameObject);
    }

    private PickupLootEvent GetEventData() {
        return new PickupLootEvent() {
            LootData = itemDropData,
            Position = transform.position
        };
    }

}