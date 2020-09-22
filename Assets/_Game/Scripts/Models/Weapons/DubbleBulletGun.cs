using System;
using UnityEngine;

[Serializable]
public class DubbleBulletGun : Weapon {

    private GameObject bulletPrefab;
    private BulletData bulletData;
    private BulletUpgradeData bulletModifierData;

    private bool isWeaponModified = false;
    private BulletData usingBulletData;

    private float spreadAngle = 20f;

    private DubbleBulletGun(WeaponData data) : base(data) {
        bulletPrefab = data.BulletData.BulletVisual;
        data.SetBulletValues();
        bulletData = data.BulletData;
        bulletModifierData = ScriptableObject.CreateInstance<BulletUpgradeData>();
        bulletModifierData.ResetValues();
        isWeaponModified = true;
    }

    private DubbleBulletGun() : this(WeaponCoordinator.GetWeaponData(WeaponType.DubbleBulletGun)) { }

    public override void Fire() {
        Quaternion rotationA = Quaternion.Euler(handPosition.rotation.eulerAngles + new Vector3(0f, spreadAngle, 0f));
        Quaternion rotationB = Quaternion.Euler(handPosition.rotation.eulerAngles + new Vector3(0f, -spreadAngle, 0f));

        Bullet_SimpleBullet bullet1 = GameObject.Instantiate(bulletPrefab, handPosition.position, rotationA).AddComponent<Bullet_SimpleBullet>();
        Bullet_SimpleBullet bullet2 = GameObject.Instantiate(bulletPrefab, handPosition.position, rotationB).AddComponent<Bullet_SimpleBullet>();

        if (isWeaponModified == true) {
            usingBulletData = GetBulletData();
            isWeaponModified = false;
        }

        bullet1.SetBulletData(usingBulletData);
        bullet2.SetBulletData(usingBulletData);
        BulletsShot += 2;
    }

    public override void ModifyWeapon(BulletUpgradeData data) {
        bulletModifierData += data;
        isWeaponModified = true;
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