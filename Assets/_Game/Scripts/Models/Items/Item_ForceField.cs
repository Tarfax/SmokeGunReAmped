using MC_Utility;
using UnityEngine;

public class Item_ForceField : Item {

    [SerializeField] private LootData_ForceField itemDropData;

    public override void SetDropItemData(LootData data) {
        itemDropData = data as LootData_ForceField;
    }

    protected override void OnPickupItem() {
        EventSystem<PickupLootEvent>.FireEvent<LootData_ForceField>(GetEventData());
        Destroy(gameObject);
    }

    protected override void OnDistanceTooFar() {
        Destroy(gameObject);
    }

    private PickupLootEvent GetEventData() {
        return new PickupLootEvent() {
            LootData = itemDropData,
            Position = transform.position
        };
    }

}
