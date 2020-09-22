using MC_Utility;
using System.Collections.Generic;
using UnityEngine;

public class AI_Behaviour : MonoBehaviour {

    private new Transform transform;

    public static List<AI_Behaviour> enemies;

    public static int spawnedAIs;
    public int ID;

    private float zKillZone = 10f;

#pragma warning disable 0649
    [SerializeField] private AI_MovementBehaviour locomotionSettings;
    [Space(2.5f)]
    [SerializeField] private AI_TargetBehaviour targetingSettings;
#pragma warning restore 0649

    void Start() {
        if (enemies == null) {
            enemies = new List<AI_Behaviour>();
        }
        enemies.Add(this);
        spawnedAIs++;
        ID = spawnedAIs;

        transform = base.transform;
        Animator animator = GetComponent<Animator>();
        locomotionSettings.Start(transform, animator);
        targetingSettings.Start(transform, animator);
    }

    void Update() {
        float deltaTime = Time.deltaTime;
        locomotionSettings.Update(deltaTime);
        targetingSettings.Update(deltaTime);

        if (transform.position.y < -50f || transform.position.z < CameraController.Position.z - zKillZone) {
            EventSystem<DeathEvent>.FireEvent(DeathEventData(DeathType.ByDistance));
            Destroy(gameObject);
        }

    }

    private void LateUpdate() {
        locomotionSettings.LateUpdate();
        targetingSettings.LateUpdate();
    }

    public DeathEvent DeathEventData(DeathType deathType) {
        DeathEvent deathEvent = new DeathEvent() {
            GameObject = gameObject,
            Position = transform.position,
            DeathType = deathType,
        };
        return deathEvent;
    }

    private void OnValidate() {
        if (Application.isPlaying) {
            locomotionSettings.OnValidate();
            targetingSettings.OnValidate();
        }
    }

    private void OnDrawGizmos() {
        targetingSettings.OnDrawGizmo();
        locomotionSettings.OnDrawGizmo(transform);
    }

    public static Transform[] GetEnemiesWithinRadius(Vector3 position, float radius) {
        List<Transform> transforms = new List<Transform>();
        if (enemies != null) {
            foreach (var AI in enemies) {
                if (Vector3.Distance(position, AI.transform.position) <= radius) {
                    transforms.Add(AI.transform);
                }
            }
        }
        return transforms.ToArray();
    }

    public static Transform GetClosestEnemy(Vector3 position) {
        float smallestDistance = float.MaxValue;
        Transform closestTransform = null;
        if (enemies.Count > 0) {
            foreach (var item in enemies) {
                float distanceTo = Vector3.Distance(item.transform.position, position);
                if (distanceTo < smallestDistance) {
                    closestTransform = item.transform;
                    smallestDistance = distanceTo;
                }
            }
        }
        return closestTransform;
    }

    private void OnDestroy() {
        enemies.Remove(this);
    }

}
