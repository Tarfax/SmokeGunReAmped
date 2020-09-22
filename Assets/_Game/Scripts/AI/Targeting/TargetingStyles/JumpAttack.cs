using MC_Utility;
using System;
using UnityEngine;

[Serializable]
public class JumpAttack : AI_Targeting {
    public override AI_TargetingType TargetingType { get => AI_TargetingType.JumpAttack; }

    [SerializeField] private float damageRadius = 3f;
    [SerializeField] private int damage = 2;

    [SerializeField] private float jumpAttackIntervall = 2f;
    private float jumpAttackIntervallTimer;

    [SerializeField] float jumpSpeed = 3f;
    [SerializeField] float jumpingDistanceThreshold = 8f;
    bool isJumping = false;

    protected override void OnInitialize() {
        jumpAttackIntervallTimer = jumpAttackIntervall;
    }

    protected override void OnUpdate() {
        jumpAttackIntervallTimer -= deltaTime;
        if (jumpAttackIntervallTimer <= 0f && Vector3.Distance(transform.position, PlayerController.Position) < jumpingDistanceThreshold) {
            jumpAttackIntervallTimer = jumpAttackIntervall;
            animator.SetTrigger(AI_Animations.Attack);
            isJumping = true;
        }

        if (isJumping == true) {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(AI_Animations.Attack) == true) {
                transform.position += transform.forward * jumpSpeed * deltaTime;
                if (animator.IsInTransition(0) == true) {
                    AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);
                    isJumping = false;
                    RaycastHit hit;
                    Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 1f, Layers.Ground);
                    EventSystem<HitEvent>.FireEvent(GetJumpHitEvent(hit));
                    EventSystem<ShakeCameraEvent>.FireEvent(new ShakeCameraEvent() { ShakeAmount = 0.4f });
                    EventSystem<AoEDamageEvent>.FireEvent(GetAoEDamageEvent());
                }
            }
        }
    }

    private AoEDamageEvent GetAoEDamageEvent() {
        return new AoEDamageEvent() {
            SourcePosition = transform.position,
            Damage = damage,
            Radius = damageRadius,
            Source = mainTransform
        };
    }

    private HitEvent GetJumpHitEvent(RaycastHit hit) {
        return new HitEvent() {
             HitData = GetHitData(hit), 
            ParticleEffectType = ParticleEffectType.GroundSmash
        };
    }
    
    private HitData GetHitData(RaycastHit hit) {
        return new HitData() {
            Damage = 5,
            DamageType = DamageType.Explosion,
            RaycastHit = hit,
            SourcePosition = transform.position
        };
    }

}