using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletsAreFun", menuName = "Combat/Bullet Definition")]
public class BulletData : ScriptableObject {

    public GameObject BulletVisual;
    private List<LayerMask> bulletTargetLayerMasks;
    public int BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float BulletDistance { get; set; }

    public void ResetValues() {
        BulletDamage = 0;
        BulletSpeed = 0f;
        BulletDistance = 0f;
    }

    public void SetLayerMasks(params int[] layers) {
        bulletTargetLayerMasks = new List<LayerMask>();
        foreach (var item in layers) {
            bulletTargetLayerMasks.Add(item);
        }
    }

    public IEnumerable<LayerMask> GetLayerMask() {
        foreach (var item in bulletTargetLayerMasks) {
            yield return item;
        }
    }

}


