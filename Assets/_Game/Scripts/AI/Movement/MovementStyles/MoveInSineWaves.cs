using System;
using UnityEngine;

[Serializable]
public class MoveInSineWaves : AI_Movement {

    public override AI_MovementType MovementType { get => AI_MovementType.SineWave; }


    [SerializeField] private float frequency = 5.0f;  // Speed of sine movement
    [SerializeField] private float magnitude = 0.2f;   // Size of sine movement

    private Vector3 axis;
    private Vector3 pos;

    protected override void OnInitialize() {
        pos = transform.position;
        axis = transform.right;
    }

    protected override void OnUpdate() {
        pos = transform.forward * deltaTime * moveSpeed;
        transform.Translate(pos + axis * Mathf.Sin(time * frequency) * magnitude, Space.World);
    }

}
