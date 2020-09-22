using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dropable", menuName = "Loot/Loot Drop Collection")]
public class LootProbables : ScriptableObject {

    public List<Probability> ItemLootProbability;

    public LootData GetItem() {
        if (ItemLootProbability != null) {
            int randomNumber = UnityEngine.Random.Range(1, 101);

            List<Probability> temp = new List<Probability>();
            foreach (var item in ItemLootProbability) {
                if (randomNumber > 100 - item.Chance) {
                    temp.Add(item);
                }
            }

            Probability loot = default;
            int lowestValue = 999;
            foreach (var item in temp) {
                if (item.Chance < lowestValue) {
                    loot = item;
                    lowestValue = item.Chance;
                }
            }
            return loot.Loot;
        }
        return null;
    }


    [Serializable]
    public struct Probability {
        public LootData Loot;
        [Range(1, 100)] public int Chance;
    }

}