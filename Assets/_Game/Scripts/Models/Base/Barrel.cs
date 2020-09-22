using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {

    private static List<Barrel> barrels;

    void Start() {
        if (barrels == null) {
            barrels = new List<Barrel>();
        }
        barrels.Add(this);
    }

    public static Transform[] GetBarrelsWithinRadius(Vector3 position, float radius) {
        List<Transform> transforms = new List<Transform>();
        foreach (var AI in barrels) {
            if (Vector3.Distance(position, AI.transform.position) <= radius) {
                transforms.Add(AI.transform);
            }
        }
        return transforms.ToArray();
    }

    private void OnDestroy() {
        barrels.Remove(this);
    }

}
