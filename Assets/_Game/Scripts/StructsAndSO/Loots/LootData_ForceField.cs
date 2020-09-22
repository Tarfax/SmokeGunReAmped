using UnityEngine;

[CreateAssetMenu(fileName = "Drop_", menuName = "Loot/Types/ForceField")]
public class LootData_ForceField : LootData {

    [Range(1, 5)] public int MinSecondsToAdd;
    [Range(1, 5)] public int MaxSecondsToAdd;

    private int secondsToAdd;

    public override void Initialize() {
        int seconds = Random.Range(MinSecondsToAdd, MaxSecondsToAdd);
        secondsToAdd = seconds;
    }

    public int GetSecondsInDrop() {
        return secondsToAdd;
    }

}
