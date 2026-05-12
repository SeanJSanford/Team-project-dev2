using UnityEngine;

public class AW_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner missing prefab or spawn points.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("Spawned test enemy.");
    }
}