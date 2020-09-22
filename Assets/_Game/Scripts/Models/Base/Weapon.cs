using System;
using UnityEngine;

public abstract class Weapon : IWeapon {

    public static int BulletsShot = 0;

    protected WeaponData weaponData;
    protected Transform handPosition;

    public float FireRate => weaponData.FireRate;
    public WeaponType WeaponType => weaponData.WeaponType;

    protected Weapon(WeaponData data) {
        weaponData = data;
    }

    public abstract void Fire();
    public abstract void ModifyWeapon(BulletUpgradeData data);

    public void Initialize(Transform hand) {
        handPosition = hand;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    public void Update() {
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public static IWeapon GetWeaponOfType<T>() where T : IWeapon {
        return (T)Activator.CreateInstance(typeof(T), true);
    }

    public static IWeapon GetWeaponOfType(Type type) {
        return (IWeapon)Activator.CreateInstance(type, true);
    }

    public static Type GetType(WeaponType type) {
        switch (type) {
            case WeaponType.SingleBulletGun:
                return typeof(SingleBulletGun);
            case WeaponType.DubbleBulletGun:
                return typeof(DubbleBulletGun);
            case WeaponType.TrippleBulletGun:
                return typeof(TrippleBulletGun);
            default: 
                return typeof(Weapon);
        }
    }

}