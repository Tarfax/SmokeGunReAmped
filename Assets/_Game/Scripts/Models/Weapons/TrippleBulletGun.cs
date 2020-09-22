using System;
using UnityEngine;
using Please = UnityEngine.Object;

[Serializable]
public class TrippleBulletGun : Weapon {

    private GameObject bulletPrefab;
    private BulletData bulletData;
    private BulletUpgradeData bulletModifierData;

    private bool isWeaponModified = false;
    private BulletData usingBulletData;

    private float spreadAngle = 25f;

    private TrippleBulletGun(WeaponData data) : base(data) {
        bulletPrefab = data.BulletData.BulletVisual;
        data.SetBulletValues();
        bulletData = data.BulletData;
        bulletModifierData = ScriptableObject.CreateInstance<BulletUpgradeData>();
        bulletModifierData.ResetValues();
        isWeaponModified = true;
    }

    private TrippleBulletGun() : this(WeaponCoordinator.GetWeaponData(WeaponType.TrippleBulletGun)) { }

    public override void Fire() {
        Quaternion rotationA = Quaternion.Euler(handPosition.rotation.eulerAngles + new Vector3(0f, spreadAngle, 0f));
        Quaternion rotationB = Quaternion.Euler(handPosition.rotation.eulerAngles + new Vector3(0f, -spreadAngle, 0f));

        Bullet_SimpleBullet bullet =  Please.Instantiate(bulletPrefab, handPosition.position, handPosition.rotation).AddComponent<Bullet_SimpleBullet>();
        Bullet_SimpleBullet bullet1 = Please.Instantiate(bulletPrefab, handPosition.position, rotationA).AddComponent<Bullet_SimpleBullet>();
        Bullet_SimpleBullet bullet2 = Please.Instantiate(bulletPrefab, handPosition.position, rotationB).AddComponent<Bullet_SimpleBullet>();

        if (isWeaponModified == true) {
            usingBulletData = GetBulletData();
            isWeaponModified = false;
        }

        bullet.SetBulletData(usingBulletData);
        bullet1.SetBulletData(usingBulletData);
        bullet2.SetBulletData(usingBulletData);

        BulletsShot += 3;
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


