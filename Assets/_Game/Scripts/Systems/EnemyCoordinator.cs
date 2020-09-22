using MC_Utility;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoordinator : MonoBehaviour {
    public static int EnemiesKilled;
    private static EnemyCoordinator instance;

    [SerializeField] private int spawnedEnemiesCount;
    [SerializeField] private int maxNumberOfEnemies = 5;

    [SerializeField] private List<EnemySpawnPoint> spawnPoints = default;
    [SerializeField] private List<GameObject> listOfEnemies = default;

    [SerializeField] private List<Transform> spawnedEnemies;

    private float spawnIntervall = 1f;
    private float spawnIntervallTimer = 1f;

    private void Awake() {
        if (instance == null) {
            instance = this;
            spawnIntervallTimer = spawnIntervall;
            spawnedEnemies = new List<Transform>();
            EventSystem<DeathEvent>.RegisterListener(OnEnemyDeath);
            spawnedEnemiesCount = 0;
            EnemiesKilled = 0;
        }
    }

    private void Update() {
        if (spawnPoints.Count > 0) {
            spawnIntervallTimer -= Time.deltaTime;
            if (spawnIntervallTimer <= 0f) {
                if (spawnedEnemiesCount < maxNumberOfEnemies) {
                    int randomInt = Random.Range(0, instance.spawnPoints.Count);
                    GameObject spawnedEnemy;
                    bool success = spawnPoints[randomInt].SpawnEnemy(out spawnedEnemy);
                    if (success == true) {
                        spawnIntervallTimer = spawnIntervall;
                        spawnedEnemiesCount++;
                        spawnedEnemies.Add(spawnedEnemy.transform);
                    }
                }
            }
        }
    }

    public static void AddSpawnPoint(EnemySpawnPoint spawnPoint) {
        if (instance.spawnPoints.Contains(spawnPoint) == false) {
            instance.spawnPoints.Add(spawnPoint);
        }
    }

    public static void RemoveSpawnPoint(EnemySpawnPoint spawnPoint) {
        if (instance.spawnPoints.Contains(spawnPoint) == true) {
            instance.spawnPoints.Remove(spawnPoint);
        }
    }

    public static GameObject GetRandomEnemy() {
        int randomInt = Random.Range(0, instance.listOfEnemies.Count);
        return instance.listOfEnemies[randomInt];
    }

    private void OnEnemyDeath(DeathEvent deathEvent) {
        spawnedEnemiesCount--;
        if (deathEvent.DeathType != DeathType.ByDistance)
            EnemiesKilled++;
        if (spawnedEnemies.Contains(deathEvent.GameObject.transform) == true) {
            spawnedEnemies.Remove(deathEvent.GameObject.transform);
        }
    }

    private void OnDisable() {
        EventSystem<DeathEvent>.UnregisterListener(OnEnemyDeath);
        instance = null;
    }

}
