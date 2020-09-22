using System;
using UnityEngine;

[Serializable]
public class AI_TargetBehaviour {

    [SerializeField] private Transform mainTransform = default;
    [SerializeField] private Transform weaponTransform = default;
    private Animator animator;
    [Space]

    [SerializeField] private AI_TargetingType targetingType = default;
    [Space]
    [SerializeField] private PeacefulOne peacefulOne = default;
    [SerializeField] private ShootStraightAhead shootStraight = default;
    [SerializeField] private ShootAndAimAtPlayer shootAtPlayer = default;
    [SerializeField] private ShootOnPlayerInSight playerInSight = default;
    [SerializeField] private JumpAttack jumpAttack = default;
    private ITargeting targeting = default;

    public void Start(Transform transform, Animator anim) {
        mainTransform = transform;
        animator = anim;
        SelectTargetingStyle();
    }

    public void SelectTargetingStyle() {
        switch (targetingType) {
            case AI_TargetingType.StraightAhead:
                targeting = shootStraight;
                break;
            case AI_TargetingType.AimAtPlayer:
                targeting = shootAtPlayer;
                break;
            case AI_TargetingType.Peaceful:
                targeting = peacefulOne;
                break;
            case AI_TargetingType.PlayerInSight:
                targeting = playerInSight;
                break;
            case AI_TargetingType.JumpAttack:
                targeting = jumpAttack;
                break;
        }

        targeting.Initialize(mainTransform, weaponTransform, animator);
    }

    public void Update(float deltaTime) {
        targeting.Update(deltaTime);
    }

    public void LateUpdate() {
        targeting.LateUpdate();
    }

    public void OnValidate() {
        if (targeting != null && targetingType != targeting.TargetingType) {
            SelectTargetingStyle();
        }
    }

    public void OnDrawGizmo() {
        playerInSight.OnDrawGizmo();
    }

}
