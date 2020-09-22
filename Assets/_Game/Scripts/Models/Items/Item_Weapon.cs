using MC_Utility;
using UnityEngine;

public class Item_Weapon : Item {

    [SerializeField] private LootData_Weapon itemDropData;

    public override void SetDropItemData(LootData data) {
        itemDropData = data as LootData_Weapon;
    }

    protected override void OnPickupItem() {
        EventSystem<PickupLootEvent>.FireEvent<LootData_Weapon>(GetEventData());
        Destroy(gameObject);
    }

    protected override void OnDistanceTooFar() {
        Destroy(gameObject);
    }

    private PickupLootEvent_Weapon GetEventData() {
        return new PickupLootEvent_Weapon() {
            LootData = itemDropData,
            Position = transform.position
        };
    }

}