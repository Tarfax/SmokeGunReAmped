using MC_Utility;
using UnityEngine;
public class Item_Money : Item {

    public static int missedMoney;

    [SerializeField] private LootData_Money itemDropData;

    public override void SetDropItemData(LootData data) {
        itemDropData = data as LootData_Money;
    }

    protected override void OnPickupItem() {
        EventSystem<PickupLootEvent>.FireEvent<LootData_Money>(GetEventData());
        Destroy(gameObject);
    }

    protected override void OnDistanceTooFar() {
        missedMoney += itemDropData.GetMoneyInDrop();
        Destroy(gameObject);
    }

    private PickupLootEvent GetEventData() {
        return new PickupLootEvent() {
            LootData = itemDropData,
            Position = transform.position
        };
    }

}
