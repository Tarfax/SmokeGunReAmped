using System;
using UnityEngine;

[Serializable]
public class SingleBulletGun : Weapon {

    private GameObject bulletPrefab;
    private BulletData bulletData;
    private BulletUpgradeData bulletModifierData;

    private BulletData usingBulletData;

    private SingleBulletGun(WeaponData data) : base(data) {
        bulletPrefab = data.BulletData.BulletVisual;
        data.SetBulletValues();
        bulletData = data.BulletData;
        bulletModifierData = ScriptableObject.CreateInstance<BulletUpgradeData>();
        bulletModifierData.ResetValues();
    }

    private SingleBulletGun() : this(WeaponCoordinator.GetWeaponData(WeaponType.SingleBulletGun)) { }

    public override void Fire() {
        Bullet_SimpleBullet bullet = GameObject.Instantiate(bulletPrefab, handPosition.position, handPosition.rotation).AddComponent<Bullet_SimpleBullet>();
        BulletData data = GetBulletData();
        bullet.SetBulletData(data);
        BulletsShot++;
    }

    public override void ModifyWeapon(BulletUpgradeData data) {
        bulletModifierData += data;
    }

    private BulletData GetBulletData() {
        BulletData modifiedBulletData = ScriptableObject.CreateInstance<BulletData>();
        modifiedBulletData.BulletVisual = bulletData.BulletVisual;
        modifiedBulletData.BulletDamage = bulletData.BulletDamage + bulletModifierData.DamageAddition;
        modifiedBulletData.BulletSpeed = bulletData.BulletSpeed * bulletModifierData.BulletSpeedMultiplier;
        modifiedBulletData.BulletDistance = bulletData.BulletDistance + bulletModifierData.BulletDistanceAddition;
        modifiedBulletData.SetLayerMasks(Layers.AI, Layers.BulletCollider);
        return modifiedBulletData;
    }

}

