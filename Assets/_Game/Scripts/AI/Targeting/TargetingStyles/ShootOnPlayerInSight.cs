using System;
using UnityEngine;

[Serializable]
public class ShootOnPlayerInSight : AI_Targeting {

    public override AI_TargetingType TargetingType { get => AI_TargetingType.PlayerInSight; }

    [Header("Player In Sight Settings")]
    [SerializeField] private float angleThreshold = 8.5f;
    //[SerializeField] private bool shouldRotateToAim = false;
    //[Range(1f, 5f)] [SerializeField] private float rotationSpeed = 2f;

    [Header("Bullet Settings")]
    [SerializeField] private GameObject BulletVisual = default;
    [Range(1, 10)] [SerializeField] private int bulletDamage = 1;
    [Range(1f, 65f)] [SerializeField] private float bulletSpeed = 25f;
    [Range(1f, 10f)] [SerializeField] private float bulletDistance = 5f;
    [Range(0.1f, 3f)] [SerializeField] private float fireRate = 0.35f;
    private float fireRateTimer;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = false;
    [SerializeField] private float angleLength = 5f;

    protected override void OnInitialize() {
        fireRateTimer = fireRate;
    }

    protected override void OnUpdate() {
        fireRateTimer -= deltaTime;
        if (fireRateTimer <= 0f) {
            Vector3 directionToPlayer = GetDirectionToPlayer();
            if (Vector3.Angle(transform.forward, directionToPlayer) <= angleThreshold) {
                Fire();
                fireRateTimer = fireRate;
            }
        }
    }

    private void Fire() {
        Vector3 forwardVector = transform.forward;
        forwardVector.y = 0f;
        Bullet_SimpleBullet bullet = UnityEngine.Object.Instantiate(BulletVisual, transform.position, Quaternion.LookRotation(forwardVector)).AddComponent<Bullet_SimpleBullet>();
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

    private Vector3 GetDirectionToPlayer() {
        Vector3 direction = (PlayerController.Position - transform.position);
        direction.y = transform.rotation.y;
        return direction;
    }

    public void OnDrawGizmo() {
        if (showGizmos == true && transform != null) {
            Vector3 plusAngle = Quaternion.AngleAxis(angleThreshold, transform.up) * transform.forward;
            Vector3 minusAngle = Quaternion.AngleAxis(-angleThreshold, transform.up) * transform.forward;

            Debug.DrawRay(transform.position, plusAngle * angleLength);
            Debug.DrawRay(transform.position, minusAngle * angleLength);

            Vector3 directionToPlayer = PlayerController.Position - transform.position;
            if (Vector3.Angle(transform.forward, directionToPlayer) <= angleThreshold) {
                Debug.DrawRay(transform.position, transform.forward * angleLength, Color.green);
            }
            else {
                Debug.DrawRay(transform.position, transform.forward * angleLength, Color.red);
            }
        }
    }

}
