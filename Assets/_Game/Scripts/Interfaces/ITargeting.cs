using UnityEngine;

public interface ITargeting {
    AI_TargetingType TargetingType { get; }
    void Initialize(Transform _mainT, Transform _, Animator __);
    void Update(float deltaTime);
    void LateUpdate();

}
