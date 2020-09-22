using System;
using UnityEngine;

[Serializable]
public abstract class AI_Movement : IMovement {

    public abstract AI_MovementType MovementType { get; }

    protected Transform transform;
    protected Animator animator;

    protected float deltaTime;
    protected float time;

    protected float moveSpeed = 3.0f;
    protected float distanceToPlayer;

    [Range(1f, 5f)] [SerializeField] private float rotationSpeed = 2f;

    public void Initialize(Transform transform, Animator anim) {
        animator = anim;
        this.transform = transform;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    public void Update(float deltaTime) {
        this.deltaTime = deltaTime;
        time += deltaTime;
        distanceToPlayer = Vector3.Distance(PlayerController.Position, transform.position);

        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void LateUpdate() {
        OnLateUpdate();
    }

    protected virtual void OnLateUpdate() { }

    public void SetMoveSpeed(float newSpeed) {
        moveSpeed = newSpeed;
    }

    protected void LookAtPlayer() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetDirectionToPlayer()), deltaTime * rotationSpeed * 100f);
    }

    protected Vector3 GetDirectionToPlayer() {
        Vector3 direction = PlayerController.Position - transform.position;
        direction.y = 0f;
        return direction;
    }

}
