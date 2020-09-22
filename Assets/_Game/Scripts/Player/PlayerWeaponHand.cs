using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHand : MonoBehaviour {

    [SerializeField] private WeaponData homingMissile = default;
    [SerializeField] private PlayerController playerController = default;

    public IWeapon WeaponInUse { get; private set; }

    private float fireRateTimer = 0f;
    private bool canFire = true;

    private void Start() {
        playerController.RegisterOnFirePressedCallback(FireWeapon);
    }

    private void Update() {
        if (WeaponInUse != null) {
            WeaponInUse.Update();
            FireCooldown();
        }
    }

    public void SwitchToWeapon(IWeapon newWeapon) {
        WeaponInUse = newWeapon;
        fireRateTimer = 0f;
    }

    private void FireCooldown() {
        if (canFire == false) {
            fireRateTimer -= Time.deltaTime;
            if (fireRateTimer <= 0f) {
                fireRateTimer = WeaponInUse.FireRate;
                canFire = true;
            }
        }
    }

    private void FireWeapon() {
        if (canFire == true && WeaponInUse != null) {
            WeaponInUse.Fire();
            canFire = false;
        }
    }

    public void FireHomingMissile() {

        Transform target = AI_Behaviour.GetClosestEnemy(transform.position);
        if (target == null) {
            return;
        }
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y / 2f, transform.position.z - 2.5f);
        Bullet_HomingMissile bullet = GameObject.Instantiate(homingMissile.BulletData.BulletVisual, startPosition, transform.rotation).AddComponent<Bullet_HomingMissile>();
        BulletData data = GetHomingMissileData(homingMissile);
        bullet.SetBulletData(data);
        bullet.SetTarget(target);
    }

    private BulletData GetHomingMissileData(WeaponData data) {
        BulletData bulletData = ScriptableObject.CreateInstance<BulletData>();
        bulletData.BulletVisual = data.BulletData.BulletVisual;
        bulletData.BulletDamage = data.BulletDamage;
        bulletData.BulletSpeed = data.BulletSpeed;
        bulletData.BulletDistance = data.DistanceUntilDeath;
        bulletData.SetLayerMasks(Layers.AI);
        return bulletData;
    }

    private void OnDisable() {
        playerController.UnregisterOnFirePressedCallback(FireWeapon);
    }

}
