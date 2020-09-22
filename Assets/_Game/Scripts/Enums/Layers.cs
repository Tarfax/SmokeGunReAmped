using UnityEngine;

public struct Layers {
    public static LayerMask Ground = 1 << 9;
    public static LayerMask Player = 1 << 10;
    public static LayerMask AI = 1 << 11;
    public static LayerMask BulletCollider = 1 << 12;
    public static LayerMask Pickup = 1 << 13;
    public static LayerMask InvisibleWall = 1 << 14;
    public static LayerMask ParticleEffects = 1 << 15;
    public static LayerMask Bullet = 1 << 16;
    public static LayerMask ForceField = 1 << 17;
}
