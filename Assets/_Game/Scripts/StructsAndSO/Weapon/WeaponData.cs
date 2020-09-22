using UnityEngine;

[CreateAssetMenu(fileName = "GunsAreFun", menuName = "Combat/Weapon Definition")]
public class WeaponData : ScriptableObject {

    [Header("Weapon Settings")]
    public WeaponType WeaponType;
    [Range(0f, 2f)] public float FireRate = 0.25f;

    [Header("Bullet Settings")]
    public BulletData BulletData;
    [Range(1, 10)] public int BulletDamage = 1;
    [Range(1f, 100f)] public float BulletSpeed = 1f;
    [Range(1f, 50f)] public float DistanceUntilDeath = 5f;

    public string WeaponDescription = "You can put ANYTHING here ;)";
    public Sprite UI_WeaponSprite = default;

    private void OnValidate() {
        FireRate = Mathf.RoundToInt(FireRate * 20f) / 20f;
        BulletSpeed = Mathf.RoundToInt(BulletSpeed * 10f) / 10f;
        DistanceUntilDeath = Mathf.RoundToInt(DistanceUntilDeath * 10f) / 10f;
    }

    public void SetBulletValues() {
        BulletData.BulletDamage = BulletDamage;
        BulletData.BulletSpeed = BulletSpeed;
        BulletData.BulletDistance = DistanceUntilDeath;
    }

}