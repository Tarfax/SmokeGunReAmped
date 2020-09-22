﻿using System;
using UnityEngine;

[Serializable]
public class ShootStraightAhead : AI_Targeting {
    public override AI_TargetingType TargetingType { get => AI_TargetingType.StraightAhead; }

    [SerializeField] private GameObject BulletVisual = default;

    [Range(1, 10)] [SerializeField] private int bulletDamage= 1;
    [Range(1f, 65f)] [SerializeField] private float bulletSpeed = 25f;
    [Range(1f, 25f)] [SerializeField] private float bulletDistance = 5f;

    [Range(0.1f, 3f)] [SerializeField] private float fireRate = 0.8f;
    private float fireRateTimer;

    protected override void OnInitialize() {
        fireRateTimer = fireRate;
    }

    protected override void OnUpdate() {
        fireRateTimer -= deltaTime;
        if (fireRateTimer <= 0f) {
            Fire();
            fireRateTimer = fireRate;
        }
    }

    private void Fire() {
        Bullet_SimpleBullet bullet = UnityEngine.Object.Instantiate(BulletVisual, transform.position, transform.rotation).AddComponent<Bullet_SimpleBullet>();
        bullet.SetBulletData(GetBulletData());
        animator.SetTrigger(AI_Animations.Attack);
    }

    private BulletData GetBulletData() {
        BulletData bullet = ScriptableObject.CreateInstance<BulletData>();
        bullet.BulletVisual = BulletVisual;
        bullet.BulletDamage = bulletDamage;
        bullet.BulletSpeed = bulletSpeed;
        bullet.BulletDistance = bulletDistance;
        bullet.SetLayerMasks(Layers.Player, Layers.BulletCollider, Layers.ForceField);
        return bullet;
    }

}