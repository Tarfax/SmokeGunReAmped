using UnityEngine;

public class DeathEvent : IEvent {
    public HealthPoints HealthPoint { get; set; }
    public GameObject GameObject { get; set; }
    public DeathType DeathType { get; set; }
    public ParticleEffectType ParticleEffectType { get; set; }
    public Vector3 Position { get; set; }
    public string SoundToPlay { get; set; }
    public float SoundCooldown { get; set; }
    public LootData GetDrop() {
        if (HealthPoint != null) {
            return HealthPoint.PropbableDrops.GetItem();
        }
        return null;
    }

}
