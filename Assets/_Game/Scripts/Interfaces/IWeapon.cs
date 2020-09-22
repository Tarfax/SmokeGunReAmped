using UnityEngine;
public interface IWeapon {
    float FireRate { get; }
    WeaponType WeaponType { get; }
    void Initialize(Transform _);
    void Update();
    void Fire();
    void ModifyWeapon(BulletUpgradeData _);
}
