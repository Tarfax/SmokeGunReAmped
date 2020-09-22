using System;
using UnityEngine;

[Serializable]
public class MoveSmart : AI_Movement {

    public override AI_MovementType MovementType { get => AI_MovementType.Smart; }

    [Header("Seeking Settings")]
    [SerializeField] private float distanceMovementMin = 7f;
    [SerializeField] private float distanceMovementMax = 20f;

    [SerializeField] private float stopForSeconds = 3.5f;
    private float stopForSecondsTimer;
    [Space]
    [SerializeField] private float moveAwayFromPlayerTime = 2.5f;
    private float moveAwayTimer;
    [Space]
    [SerializeField] private float regularMovementUntilPause = 3.5f;
    [SerializeField] private float pauseInMovementTime = 0.75f;
    private float regularMoveTimer;

    private bool shouldMove;
    private Vector3 forward;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = false;
    [SerializeField] private float forwardLength = 4f;
    [SerializeField] private float distanceToPlayerGizmo = 4f;

    protected override void OnInitialize() {
        stopForSecondsTimer = stopForSeconds;
        shouldMove = true;
        SetForwardDirection(distanceToPlayer, GetDirectionToPlayer());
    }

    protected override void OnUpdate() {
        if (shouldMove == false) {
            stopForSecondsTimer -= deltaTime;
            Vector3 directionToPlayer = GetDirectionToPlayer();
            if (stopForSecondsTimer <= 0f) {
                moveAwayTimer -= deltaTime;
                if (moveAwayTimer > 0f && distanceToPlayer < distanceMovementMin) {
                    SetForwardDirection(distanceToPlayer, directionToPlayer);
                    transform.position += forward.normalized * moveSpeed * deltaTime;
                }
                else {
                    moveAwayTimer = moveAwayFromPlayerTime;
                    stopForSecondsTimer = stopForSeconds;
                    shouldMove = true;
                    animator.SetBool(AI_Animations.Move, true);
                }
            }
            else {
                LookAtPlayer();
                return;
            }
        }

        if (shouldMove == true) {
            if (distanceToPlayer < distanceMovementMin) {
                shouldMove = false;
                animator.SetBool(AI_Animations.Move, false);
            }
            else if (distanceToPlayer > distanceMovementMax) {
                Vector3 playerDirection = GetDirectionToPlayer();
                SetForwardDirection(distanceToPlayer, playerDirection);
                LookAtPlayer();
                transform.position += forward.normalized * moveSpeed * deltaTime;
            }
            else {
                regularMoveTimer -= deltaTime;
                if (regularMoveTimer >= 0f) {
                    LookAtPlayer();
                    transform.position += forward.normalized * moveSpeed * deltaTime;
                }
                else {
                    shouldMove = false;
                    animator.SetBool(AI_Animations.Move, false);
                    regularMoveTimer = regularMovementUntilPause;
                    stopForSecondsTimer = pauseInMovementTime;
                    Vector3 playerDirection = GetDirectionToPlayer();
                    SetForwardDirection(distanceToPlayer, playerDirection);
                }
            }

        }
    }
    
    private void SetForwardDirection(float distance, Vector3 playerDirection) {
        if (distance < distanceMovementMin) {
            forward = -playerDirection;
        }
        else {
            forward = playerDirection;
        }
        forward.y = 0f;
    }

    public void OnDrawGizmo() {
        if (showGizmos == true) {
            distanceToPlayerGizmo = Vector3.Distance(PlayerController.Position, transform.position);
            Vector3 playerDirectionPlayerFirst = GetDirectionToPlayer();

            Debug.DrawRay(transform.position, playerDirectionPlayerFirst * forwardLength, Color.yellow);

            if (distanceToPlayerGizmo < distanceMovementMin) {
                Debug.DrawRay(transform.position + Vector3.up * 0.1f, -playerDirectionPlayerFirst * forwardLength, Color.green);
            }
            else if (distanceToPlayerGizmo > distanceMovementMax) {
                Debug.DrawRay(transform.position + Vector3.up * 0.2f, playerDirectionPlayerFirst * forwardLength, Color.red);
            }

        }
    }

}