using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;           // 소환할 적 프리팹
    public Transform[] spawnPoints;          // 소환 지점들
    public float spawnInterval = 5f;         // 소환 주기 (초 단위)

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("SpawnPoints or EnemyPrefab is not set.");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
