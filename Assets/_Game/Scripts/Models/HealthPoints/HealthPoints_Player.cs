using MC_Utility;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoints_Player : HealthPoints {

    [SerializeField] private Text hpValue = default;

    [SerializeField] private int healthPoints = 3;
    private bool canTakeDamage = true;

    [SerializeField] private Material material = default;
    float timer;
    float invunerableTime = 3f;

    private void Start() {
        hpValue.text = healthPoints.ToString();
        timer = 0f;
    }

    private void Update() {

        if (canTakeDamage == false) {
            timer += Time.deltaTime * 5f;
            float sin = Mathf.Abs(Mathf.Sin(timer));
            Color red = material.color;
            red.b = sin;
            red.g = sin;
            material.color = red;
        }
        else if (timer > 0f) {
            timer = 0f;
            material.color = Color.white;
        }
    }

    public override void DoDamage(HitData hitData) {
        if (isAlive == true) {
            if (canTakeDamage == true) {
                healthPoints -= hitData.Damage;
                hpValue.text = healthPoints.ToString();
                if (healthPoints <= 0) {
                    EventSystem<DeathEvent_Player>.FireEvent(GetDeathEventData());
                    isAlive = false;
                    GameInstance.GameOver();
                }
                GameInstance.InvokeFunction(CanTakeDamageAgain, invunerableTime);
                canTakeDamage = false;
            }
        }
    }

    protected DeathEvent_Player GetDeathEventData() {
        DeathEvent_Player barrelDeathEvent = new DeathEvent_Player() {
            HealthPoint = this,
            Position = transform.position,
            DeathType = DeathType.BarrelExplosion,
            ParticleEffectType = ParticleEffectType.BodyFleshExplosion
        };
        return barrelDeathEvent;
    }

    private void CanTakeDamageAgain() {
        canTakeDamage = true;
    }

    private void OnDestroy() {
        material.color = Color.white;
    }

}
