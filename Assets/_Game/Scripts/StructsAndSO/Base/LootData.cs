using UnityEngine;

public abstract class LootData : ScriptableObject {

    public GameObject Visual;
    public virtual void Initialize() { }
}