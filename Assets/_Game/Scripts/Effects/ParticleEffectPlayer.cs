using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPlayer : MonoBehaviour {
    public bool testParticles;
    public bool playOnStart = false;
    [Header("For exploding body parts only")]
    [SerializeField] private GameObject goreExplosion = default;

    [Header("All the particles")]
    [SerializeField] private List<ParticleSystem> particlesToPlay = default;
    public ParticleEffectType ParticleEffectType { get; set; }

    private void Start() {
        if (playOnStart) {
            PlayParticles();
        }
    }

    public void PlayParticles() {
        foreach (var item in particlesToPlay) {
            item.Play();
        }

        if (goreExplosion != null) {
            GameObject gameObject = Instantiate(goreExplosion, transform.position, Quaternion.Euler(Vector3.zero));
            gameObject.transform.LookAt(PlayerController.Position, Vector3.up);
            gameObject.transform.SetParent(transform, true);
            GoreExploder goreExploder = gameObject.GetComponent<GoreExploder>();
            goreExploder.Initialize();
            goreExploder.Explode();
        }

    }

    private void OnValidate() {
        if (testParticles == true) {
            PlayParticles();
            testParticles = false;
        }
    }

}
