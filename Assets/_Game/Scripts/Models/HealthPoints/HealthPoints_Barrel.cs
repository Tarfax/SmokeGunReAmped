using MC_Utility;
using UnityEngine;

public class HealthPoints_Barrel : HealthPoints {

    public static int barrelsDestroyed;

    [SerializeField] private int healthPoints;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int explosionDamage = 2;

    public override void DoDamage(HitData hitData) {
        if (isAlive == true) {
            healthPoints -= hitData.Damage;
            EventSystem<HitEvent>.FireEvent(GetHitEventData(hitData));
            if (healthPoints <= 0) {
                isAlive = false;
                EventSystem<ShakeCameraEvent>.FireEvent(new ShakeCameraEvent() { ShakeAmount = 0.55f });
                EventSystem<DeathEvent>.FireEvent(GetDeathEventData(hitData.DamageType));
                EventSystem<AoEDamageEvent>.FireEvent(GetAoEDamageEvent());
                Destroy(gameObject);
                barrelsDestroyed++;
            }
        }
    }

    private AoEDamageEvent GetAoEDamageEvent() {
        return new AoEDamageEvent() {
            SourcePosition = transform.position,
            Damage = explosionDamage,
            Radius = explosionRadius,
            Source = gameObject.transform
        };
    }

    protected DeathEvent GetDeathEventData(DamageType damageType) {
        DeathEvent barrelDeathEvent = new DeathEvent() {
            GameObject = gameObject,
            HealthPoint = this,
            Position = transform.position,
            DeathType = DeathType.BarrelExplosion,
            ParticleEffectType = ParticleEffectType.BarrelExplosion
        };
        return barrelDeathEvent;
    }

    protected HitEvent GetHitEventData(HitData hitData) {
        return new HitEvent() {
            ParticleEffectType = ParticleEffectType.BulletImpactMetal,
            HitData = hitData
        };
    }

}
