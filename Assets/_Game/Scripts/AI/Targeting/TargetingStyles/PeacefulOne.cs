using System;
using UnityEngine;

[Serializable]
public class PeacefulOne : AI_Targeting {
    public override AI_TargetingType TargetingType { get => AI_TargetingType.Peaceful; }

    [SerializeField] private bool lookAtPlayer = false;

    protected override void OnUpdate() {
        if (lookAtPlayer == true) {
            transform.rotation = Quaternion.LookRotation(PlayerController.Position - transform.position);
        }
    }

}
