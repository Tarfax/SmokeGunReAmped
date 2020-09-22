using UnityEngine;

[CreateAssetMenu(fileName = "Drop_", menuName = "Loot/Types/Money")]
public class LootData_Money : LootData {

    [Header("The number will be multiplied with 100kr")]
    [Range(1, 10)] public int MoneyMin;
    [Range(1, 10)] public int MoneyMax;

    private int moneyInDrop;

    public override void Initialize() {
        int money = Random.Range(MoneyMin, MoneyMax);
        moneyInDrop = money * 100;
    }

    public int GetMoneyInDrop() {
        return moneyInDrop;
    }

}
