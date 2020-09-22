using UnityEngine;

public abstract class HealthPoints : MonoBehaviour {

    protected bool isAlive = true;
    public bool IsAlive { get => isAlive; }
    public abstract void DoDamage(HitData hitData);

    [SerializeField] protected LootProbables drops = default;
    public LootProbables PropbableDrops {
        get {
            if (drops == null) return ScriptableObject.CreateInstance<LootProbables>(); else { return drops; }
        }
    }

}
