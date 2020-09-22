using MC_Utility;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsCoordinator : MonoBehaviour {

    private Dictionary<GameObject, float> particlesToKill;
    private int activeParticlesCount = 0;
    private Dictionary<ParticleEffectType, Queue<GameObject>> objectPool;
    private Dictionary<ParticleEffectType, ParticleData> particleEffects;
    [SerializeField] private List<ParticleData> particles = default;

    private void Start() {
        objectPool = new Dictionary<ParticleEffectType, Queue<GameObject>>();
        particlesToKill = new Dictionary<GameObject, float>();
        particleEffects = new Dictionary<ParticleEffectType, ParticleData>();

        particleEffects.Add(ParticleEffectType.None, null);

        foreach (ParticleData item in particles) {
            particleEffects.Add(item.ParticleEffectType, item);
            objectPool.Add(item.ParticleEffectType, new Queue<GameObject>());
        }

        EventSystem<DeathEvent>.RegisterListener(OnDeathEvent);
        EventSystem<DeathEvent_Player>.RegisterListener(OnPlayerDeathEvent);
        EventSystem<HitEvent>.RegisterListener(OnHitEvent);
    }

    private void Update() {
        if (activeParticlesCount > 0) {
            float deltaTime = Time.deltaTime;
            bool disableParticles = false;

            Dictionary<GameObject, float> temp = new Dictionary<GameObject, float>(particlesToKill);
            foreach (var key in temp.Keys) {
                particlesToKill[key] -= deltaTime;
                if (particlesToKill[key] <= 0f) {
                    disableParticles = true;
                }
            }

            if (disableParticles == true) {
                Dictionary<GameObject, float> temp2 = new Dictionary<GameObject, float>(particlesToKill);
                foreach (GameObject key in temp2.Keys) {
                    if (temp2[key] <= 0f) {
                        particlesToKill.Remove(key);
                        ParticleEffectType type = key.GetComponent<ParticleEffectPlayer>().ParticleEffectType;
                        objectPool[type].Enqueue(key);
                        key.SetActive(false);
                    }
                }
                activeParticlesCount = particlesToKill.Count;
            }
        }
    }

    private void OnPlayerDeathEvent(DeathEvent_Player deathEvent) {
        GameObject gameObject = GetParticleOfType(deathEvent.ParticleEffectType, particleEffects[deathEvent.ParticleEffectType].ParticleEffect);
        gameObject.SetActive(true);
        gameObject.transform.position = deathEvent.Position;
        gameObject.transform.rotation = Random.rotation;
        PlayParticle(gameObject, particleEffects[deathEvent.ParticleEffectType].ParticleDuration);
    }

    private void OnDeathEvent(DeathEvent data) {
        if (data.ParticleEffectType == ParticleEffectType.None) {
            return;
        }
        GameObject gameObject = GetParticleOfType(data.ParticleEffectType, particleEffects[data.ParticleEffectType].ParticleEffect);
        gameObject.SetActive(true);
        gameObject.transform.position = data.Position;
        gameObject.transform.rotation = Random.rotation;
        PlayParticle(gameObject, particleEffects[data.ParticleEffectType].ParticleDuration);
    }

    private void OnHitEvent(HitEvent data) {
        if (data.ParticleEffectType == ParticleEffectType.None) {
            return;
        }
        GameObject gameObject = GetParticleOfType(data.ParticleEffectType, particleEffects[data.ParticleEffectType].ParticleEffect);
        gameObject.SetActive(true);
        gameObject.transform.position = data.HitData.RaycastHit.point;
        Vector3 direction = data.HitData.RaycastHit.point - data.HitData.SourcePosition;
        if (direction != Vector3.zero) {
            gameObject.transform.rotation = Quaternion.LookRotation(direction.normalized);
        }
        PlayParticle(gameObject, particleEffects[data.ParticleEffectType].ParticleDuration);
    }

    private GameObject GetParticleOfType(ParticleEffectType type, GameObject objectToInstatiate) {
        if (objectPool[type].Count > 0) {
            return objectPool[type].Dequeue();
        }
        else {
            GameObject gameObject = Instantiate(objectToInstatiate);
            gameObject.GetComponent<ParticleEffectPlayer>().ParticleEffectType = type;
            return gameObject;
        }
    }

    private void PlayParticle(GameObject gameObject, float duration) {
        ParticleEffectPlayer pep = gameObject.GetComponent<ParticleEffectPlayer>();
        pep.PlayParticles();
        particlesToKill.Add(gameObject, duration);
        activeParticlesCount = particlesToKill.Count;
    }

    private void OnDisable() {
        EventSystem<DeathEvent>.UnregisterListener(OnDeathEvent);
        EventSystem<DeathEvent_Player>.UnregisterListener(OnPlayerDeathEvent);
        EventSystem<HitEvent>.UnregisterListener(OnHitEvent);
    }

}
