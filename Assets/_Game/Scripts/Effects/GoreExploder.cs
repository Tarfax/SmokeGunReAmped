using System.Collections.Generic;
using UnityEngine;

public class GoreExploder : MonoBehaviour {

    public GameObject explodeFX;

    public GameObject intestinesA;
    public Transform intestinesARig;
    private Material intenstinesAMaterial;
    private Rigidbody intestinesARigidBody;

    public GameObject heart;
    private Material heartMaterial;
    private Rigidbody heartRigidBody;

    #region ForMoreEffect - But was eating FPS
    //public GameObject intestinesB;
    //public Transform intestinesBRig;
    //private Material intenstinesBMaterial;
    //private Rigidbody intestinesBRigidBody;
    #endregion

    private bool doFade = false;

    private Color resetColor = Color.white;

    [SerializeField] private List<ParticleSystem> particles = default;
    private float fadeBeginTimer = 5f;

    // Use this for initialization
    public void Initialize() {
        fadeBeginTimer = 5f;

        intestinesA.transform.rotation = Quaternion.Euler(Vector3.up);
        intestinesARigidBody = intestinesARig.GetComponent<Rigidbody>();
        intenstinesAMaterial = intestinesA.GetComponentInChildren<SkinnedMeshRenderer>().material;
        intestinesARigidBody.velocity = Vector3.zero;
        intenstinesAMaterial.color = resetColor;

        heartRigidBody = heart.GetComponent<Rigidbody>();
        heartMaterial = heart.GetComponent<MeshRenderer>().material;
        heartRigidBody.velocity = Vector3.zero;
        heartMaterial.color = resetColor;
    }

    public void Explode() {
        // Launch dymanic flesh pieces
        explodeFX.SetActive(true);
        intestinesARigidBody.AddForce(Random.Range(0, 10), Random.Range(5, 10), Random.Range(0, 10), ForceMode.Impulse);
        heartRigidBody.AddForce(Random.Range(0, 5), Random.Range(10, 20), Random.Range(0, 5), ForceMode.Impulse);
        foreach (var item in particles) {
            item.Play();
        }
        doFade = true;
    }

    private void Update() {
        if (doFade == true) {
            if (fadeBeginTimer > 0f) {
                fadeBeginTimer -= Time.deltaTime;
            }
            else {
                FadeOut();
            }
        }
    }

    private void FadeOut() {
        if (intenstinesAMaterial.color.a >= 0.05) {
            Color colorA = intenstinesAMaterial.color;  //The variable "color" is the renderers material color
            Color colorC = heartMaterial.color;  //The variable "color" is the renderers material color

            if (colorA.a >= 0) {
                colorA.a -= 0.05f;
                colorC.a -= 0.05f;
                intenstinesAMaterial.color = colorA;
                heartMaterial.color = colorC;

                intestinesARigidBody.velocity = Vector3.Lerp(Vector3.zero, intestinesARigidBody.velocity, Time.deltaTime /2f);
            }
        }
        else {
            Destroy(gameObject);
        }
    }

}

