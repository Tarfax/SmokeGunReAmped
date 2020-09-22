using UnityEngine;

public class PickupLootEvent : IEvent {
    public LootData LootData;
    public Vector3 Position;
}
