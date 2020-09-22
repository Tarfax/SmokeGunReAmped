using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PlayerForceField : MonoBehaviour {

    [SerializeField] private SphereCollider sphereCollider = default;
    [SerializeField] private MeshRenderer meshRenderer = default;

    public static int AbsorbedBullets;

    private float hitTime;
    private Material material;

    void Start() {
        material = GetComponent<Renderer>().sharedMaterial;
        AbsorbedBullets = 0;
    }

    void Update() {
        if (hitTime > 0) {
            float timer = Time.deltaTime * 1000;
            hitTime -= timer;
            if (hitTime < 0) {
                hitTime = 0;
            }
            material.SetFloat("_HitTime", hitTime);
        }
    }

    public void BulletHit(RaycastHit hit) {
        material.SetVector("_HitPosition", transform.InverseTransformPoint(hit.point));
        hitTime = 500;
        material.SetFloat("_HitTime", hitTime);
        AbsorbedBullets++;
    }

    public void ActivateShield() {
        sphereCollider.enabled = true;
        meshRenderer.enabled = true;
    }

    public void DeactivateShield() {
        sphereCollider.enabled = false;
        meshRenderer.enabled = false;
    }

}