using MC_Utility;
using UnityEngine;

public class HealthPoints_AI : HealthPoints {

    [SerializeField] private int healthPoints;

    public override void DoDamage(HitData hitData) {
        if (isAlive == true) {
            healthPoints -= hitData.Damage;
            EventSystem<HitEvent>.FireEvent(HitEventData(hitData));
            if (healthPoints <= 0) {
                EventSystem<DeathEvent>.FireEvent(DeathEventData(DeathTypeByDamageType(hitData.DamageType)));
                isAlive = false;
                Destroy(gameObject, 0.1f);
            }
        }
    }

    public DeathEvent DeathEventData(DeathType deathType) {
        DeathEvent deathEvent = new DeathEvent() {
            GameObject = gameObject,
            HealthPoint = this,
            Position = transform.position,
            DeathType = deathType,
            ParticleEffectType = ParticleEffectType.BodyFleshExplosion
        };
        return deathEvent;
    }

    public HitEvent HitEventData(HitData bulletHit) {
        HitEvent deathEvent = new HitEvent() {
            ParticleEffectType = ParticleEffectType.BloodSplatter,
            HitData = bulletHit
        };
        return deathEvent;
    }


    public static DeathType DeathTypeByDamageType(DamageType damageType) {
        switch (damageType) {
            case DamageType.Explosion:
                return DeathType.HomingMissile;
            case DamageType.Bullet:
            default:
                return DeathType.RegularBullet;
        }
    }

}