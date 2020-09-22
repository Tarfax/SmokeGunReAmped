using UnityEngine;

public class Bullet_SimpleBullet : Bullet {

    protected override void Move(float deltaTime) {
            transform.Translate(transform.forward * bulletData.BulletSpeed * deltaTime, Space.World);
    }

    protected override void OnCollision(RaycastHit hit) {
        PlayerForceField forceField = hit.collider.gameObject.GetComponent<PlayerForceField>();
        if (forceField != null) {
            forceField.BulletHit(hit);
        }

        HealthPoints hp = hit.collider.GetComponent<HealthPoints>();
        if (hp != null && hp.IsAlive == true) {
            hp.DoDamage(GetBulletHitData(hit, bulletData.BulletDamage));
        }
    }

    protected override HitData GetBulletHitData(RaycastHit hit, int bulletDamage) {
        return new HitData() {
            SourcePosition = startingPosition,
            RaycastHit = hit,
            DamageType = DamageType.Bullet,
            Damage = bulletDamage
        };
    }

}