using UnityEngine;

public class SphereGizmo : MonoBehaviour {
    [SerializeField] private float radius = 1;
    [SerializeField] private float activationRadius = 10;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool drawWireSphere = false;
    [SerializeField] private bool showActivationRadius = default;

    private void OnDrawGizmosSelected() {
        Gizmos.color = color;
        if (drawWireSphere == true)
            Gizmos.DrawWireSphere(transform.position, radius);
        else
            Gizmos.DrawSphere(transform.position, radius);

        if (showActivationRadius == true) {

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, activationRadius);
        }

    }
}
