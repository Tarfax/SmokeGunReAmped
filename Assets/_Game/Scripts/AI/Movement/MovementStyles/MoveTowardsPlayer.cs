using System;
using UnityEngine;

[Serializable]
public class MoveTowardsPlayer : AI_Movement {
    public override AI_MovementType MovementType { get => AI_MovementType.TowardsPlayer; }

    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float stopTime = 2f;
    private float stopTimer;
    private bool isWalking;

    protected override void OnInitialize() {
        stopTimer = stopTime;
    }

    protected override void OnUpdate() {
        LookAtPlayer();
        if (isWalking == true) {
            if (Vector3.Distance(transform.position, PlayerController.Position) > stopDistance) {
                transform.position += transform.forward * moveSpeed * deltaTime;
            } else {
                isWalking = false;
                animator.SetBool(AI_Animations.Move, isWalking);
            }
        }

        if (isWalking == true) {
            stopTimer -= deltaTime;
            if (stopTimer <= 0f) {
                isWalking = true;
                animator.SetBool(AI_Animations.Move, isWalking);
                stopTimer = stopTime;
            }
        }

    }


}