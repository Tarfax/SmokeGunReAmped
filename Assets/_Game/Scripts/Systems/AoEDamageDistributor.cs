using MC_Utility;
using UnityEngine;

public class AoEDamageDistributor : MonoBehaviour {

    void Start() {
        EventSystem<AoEDamageEvent>.RegisterListener(DamageAllThingsNearby);
    }

    private void DamageAllThingsNearby(AoEDamageEvent aoeEvent) {
        Transform[] all = CollectAllTransforms(aoeEvent);

        foreach (Transform transform in all) {
            if (transform != aoeEvent.Source && transform != null) {
                HealthPoints hp = transform.GetComponent<HealthPoints>();
                if (hp != null && hp.IsAlive == true) {
                    hp.DoDamage(GetHitData(aoeEvent));
                }
            }
        }
    }

    private Transform[] CollectAllTransforms(AoEDamageEvent aoeEvent) {
        Transform[] aiTransforms = AI_Behaviour.GetEnemiesWithinRadius(aoeEvent.SourcePosition, aoeEvent.Radius);
        Transform[] barrelTransforms = Barrel.GetBarrelsWithinRadius(aoeEvent.SourcePosition, aoeEvent.Radius);
        Transform[] all = new Transform[aiTransforms.Length + barrelTransforms.Length + 1];
        aiTransforms.CopyTo(all, 0);
        barrelTransforms.CopyTo(all, 0);

        if (Vector3.Distance(PlayerController.Position, aoeEvent.SourcePosition) < aoeEvent.Radius) {
            Transform playerTransform = PlayerController.GameObject.transform;
            all[all.Length - 1] = playerTransform;
        } 

        return all;
    }

    private HitData GetHitData(AoEDamageEvent aoeEvent) {
        return new HitData() {
            Damage = aoeEvent.Damage,
            DamageType = DamageType.Collateral,
            SourcePosition = aoeEvent.SourcePosition,
            RaycastHit = new RaycastHit() { point = aoeEvent.SourcePosition }
        };
    }

    private void OnDisable() {
        EventSystem<AoEDamageEvent>.UnregisterListener(DamageAllThingsNearby);
    }

}