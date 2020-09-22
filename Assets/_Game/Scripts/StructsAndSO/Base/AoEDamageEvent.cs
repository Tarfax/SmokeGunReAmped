using UnityEngine;

public class AoEDamageEvent : IEvent {
    public Transform Source;
    public Vector3 SourcePosition;
    public float Radius;
    public int Damage;
}