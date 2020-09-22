using System;
using UnityEngine;

[Serializable]
public class StandingStill : AI_Movement {
    public override AI_MovementType MovementType { get => AI_MovementType.StandingStill; }

    [SerializeField] private bool lookAtPlayer = true;

    protected override void OnInitialize() {
        animator.SetBool(AI_Animations.Move, false);
    }

    protected override void OnUpdate() {
        if (lookAtPlayer == true) {
            transform.rotation = Quaternion.LookRotation(PlayerController.Position - transform.position);
        }
    }

}