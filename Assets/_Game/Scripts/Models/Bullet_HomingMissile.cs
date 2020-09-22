using MC_Utility;
using UnityEngine;
public class Bullet_HomingMissile : Bullet {

    private Transform target;

    private float speed;
    private float targetSpeed;
    private float accelerationSpeed = 4f;
    private float rotationSpeed = 4f;

    protected override void Move(float deltaTime) {
        if (target != null) {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.forward).y * rotationSpeed;
            transform.Rotate(Vector3.up, -rotateAmount);
            speed = Mathf.MoveTowards(speed, targetSpeed, deltaTime * accelerationSpeed);
            transform.Translate(transform.forward * speed * deltaTime, Space.World);
        } else {
            target = AI_Behaviour.GetClosestEnemy(transform.position);
        }
    }

    protected override void OnCollision(RaycastHit hit) {
        HealthPoints hp = hit.collider.GetComponent<HealthPoints>();
        if (hp != null && hp.IsAlive == true) {
            hp.DoDamage(GetBulletHitData(hit, bulletData.BulletDamage));
            EventSystem<HitEvent>.FireEvent(GetHitEventData(hit));
            EventSystem<ShakeCameraEvent>.FireEvent(new ShakeCameraEvent() { ShakeAmount = 0.70f });
            EventSystem<AoEDamageEvent>.FireEvent(GetAoEDamageEvent());
        }
    }

    private AoEDamageEvent GetAoEDamageEvent() {
        return new AoEDamageEvent() {
            SourcePosition = transform.position,
            Damage = 1,
            Radius = 3f,
            Source = transform
        };
    }

    private HitEvent GetHitEventData(RaycastHit hit) {
        return new HitEvent() {
            HitData = GetBulletHitData(hit, bulletData.BulletDamage),
            ParticleEffectType = ParticleEffectType.BarrelExplosion
        };
    }

    protected override HitData GetBulletHitData(RaycastHit hit, int bulletDamage) {
        return new HitData() {
            SourcePosition = startingPosition,
            RaycastHit = hit,
            DamageType = DamageType.Explosion,
            Damage = bulletDamage
        };
    }

    public void SetTarget(Transform target) {
        this.target = target;
        targetSpeed = bulletData.BulletSpeed;
    }
}
