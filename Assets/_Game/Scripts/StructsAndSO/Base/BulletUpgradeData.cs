using UnityEngine;
public class BulletUpgradeData : ScriptableObject {

    [Range(1f, 5f)] public float FireRateMultiplier = 1f;
    [Range(0, 5)] public int DamageAddition = 0;
    [Range(1f, 5f)] public float BulletSpeedMultiplier = 1;
    [Range(0f, 15f)] public float BulletDistanceAddition = 0f;

    public void ResetValues() {
        FireRateMultiplier = 1f;
        DamageAddition = 0;
        BulletSpeedMultiplier = 1;
        BulletDistanceAddition = 0f;
    }

    public static BulletUpgradeData operator +(BulletUpgradeData left, BulletUpgradeData right) {
        left.FireRateMultiplier *= right.FireRateMultiplier;
        left.DamageAddition += right.DamageAddition;
        left.BulletSpeedMultiplier *= right.BulletSpeedMultiplier;
        left.BulletDistanceAddition += right.BulletDistanceAddition;
        return left;
    }

    public static BulletUpgradeData operator -(BulletUpgradeData left, BulletUpgradeData right) {
        left.FireRateMultiplier *= right.FireRateMultiplier;
        left.DamageAddition -= right.DamageAddition;
        left.BulletSpeedMultiplier *= right.BulletSpeedMultiplier;
        left.BulletDistanceAddition -= right.BulletDistanceAddition;
        return left;
    }

}