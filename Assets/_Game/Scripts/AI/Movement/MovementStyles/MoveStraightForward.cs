using System;
using UnityEngine;

[Serializable]
public class MoveStraightForward : AI_Movement {

    public override AI_MovementType MovementType { get => AI_MovementType.StraightForward; }

    [SerializeField] private float stopIntervall = 0.5f;
    [SerializeField] private float stopForSeconds = 0.5f;

    private float stopIntervallTimer;
    private float stopForSecondsTimer;

    private bool isStopped;

    private Vector3 forward;

    private float distanceMovement = 11f;

    protected override void OnInitialize() {
        stopIntervallTimer = stopIntervall;

        stopForSecondsTimer = stopForSeconds;
        int directionChange = UnityEngine.Random.Range(0, 1);
        if (directionChange == 0) {
            if (transform != null) {
                forward = transform.forward;
            }
        } else {

        }
    }

    protected override void OnUpdate() {

        if (isStopped == true) {
            stopForSecondsTimer -= deltaTime;
            if (stopForSecondsTimer <= 0f) {
                stopForSecondsTimer = stopForSeconds;
                isStopped = false;
                animator.SetBool(AI_Animations.Move, true);
                CalculateMovementDirection(distanceToPlayer);
            }
            else {
                return;
            }
        }

        if (isStopped == false) {
            if (distanceToPlayer < distanceMovement) {
                stopIntervallTimer -= deltaTime * 3f;
            }
            else {
                stopIntervallTimer -= deltaTime;
            }

            if (stopIntervallTimer <= 0f) {
                isStopped = true;
                animator.SetBool(AI_Animations.Move, false);
                stopIntervallTimer = stopIntervall;
                return;
            }

            transform.position += forward * moveSpeed * deltaTime;
        }

    }

    private void CalculateMovementDirection(float distance) {
        if (distance < distanceMovement) {
            forward = -transform.forward;
        }
        else {
            forward = transform.forward;
        }
    }

}
