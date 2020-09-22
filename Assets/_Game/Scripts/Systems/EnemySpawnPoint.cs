using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnPoint : MonoBehaviour {

    private new Transform transform;

    [Header("Settings")]
    [SerializeField] private bool lookAtCamera = false;

    [Header("Enemies")]
    [SerializeField] private List<EnemySpawnChance> enemiesToSpawn = default;

    [Header("Debug Tools")]
    [SerializeField] private bool isActive;
    private float startDistance;

    private void Start() {
        isActive = true;
        transform = base.transform;

        startDistance = Mathf.Abs(transform.position.z - CameraController.Position.z);

        EnemyCoordinator.AddSpawnPoint(this);
    }

    private void Update() {
        if (isActive == true) {

            if (lookAtCamera == true) {
                Vector3 cameraDirection = CameraController.CameraMidPoint.position - transform.position;
                transform.rotation = Quaternion.LookRotation(cameraDirection);
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
            }

            Vector3 newPosition = transform.position;
            newPosition.z = startDistance + CameraController.Position.z;
            transform.position = newPosition;
        }
    }

    public bool SpawnEnemy(out GameObject spawnedEnemy) {
        if (isActive == true) {
            EnemySpawnChance randomSelectedEnemy;
            int randomIndex = Random.Range(0, enemiesToSpawn.Count);
            randomSelectedEnemy = enemiesToSpawn[randomIndex];

            int randomChance = Random.Range(0, 10) + 1;
            if (randomSelectedEnemy.SpawnChance >= randomChance) {
                spawnedEnemy = Instantiate(randomSelectedEnemy.EnemyToSpawn, transform.position, Quaternion.LookRotation(transform.forward));
                return true;
            }
        }
        spawnedEnemy = null;
        return false;
    }

    [Serializable]
    public struct EnemySpawnChance {
        [Range(1, 10)] public int SpawnChance;
        public GameObject EnemyToSpawn;
    }

}
