using UnityEngine;

public abstract class AI_Targeting : ITargeting {
    public abstract AI_TargetingType TargetingType { get; }

    protected Transform mainTransform;
    protected Transform transform;
    protected Animator animator;
    protected float deltaTime;

    public void Initialize(Transform ai_transform, Transform transform, Animator anim) {
        this.mainTransform = ai_transform;
        animator = anim;
        this.transform = transform;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    public void Update(float deltaTime) {
        this.deltaTime = deltaTime;
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void LateUpdate() {
        OnLateUpdate();
    }
    protected virtual void OnLateUpdate() { }

}
