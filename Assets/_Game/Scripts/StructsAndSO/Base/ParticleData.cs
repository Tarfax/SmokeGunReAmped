using UnityEngine;

[CreateAssetMenu(fileName ="Effect", menuName = "Effects/Particle Effect Definition")]
public class ParticleData : ScriptableObject {

    public ParticleEffectType ParticleEffectType;
    public GameObject ParticleEffect;
    public float ParticleDuration = 3f;

}
