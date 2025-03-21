using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyConfig
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnRate;
    public float enemySpeed;
}

[System.Serializable]
public struct EnemyWaveConfig
{
    public EnemyConfig[] enemies;
}

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Wave Configurations")]
    [SerializeField] private EnemyWaveConfig[] waves;
    [SerializeField] private float timeBetweenWaves = 3f;

    private int _currentWave;
    private readonly List<EnemyMovement> _activeEnemies = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeSpawner()
    {
        if (waves == null || waves.Length == 0)
        {
            Debug.LogWarning("No waves configured. Skipping spawner initialization.");
            LevelManager.Main?.LoadNextLevel();
            return;
        }

        StopAllCoroutines();
        _currentWave = 0;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (_currentWave < waves.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            EnemyWaveConfig wave = waves[_currentWave];
            foreach (var enemyConfig in CreateEnemyQueue(wave))
            {
                SpawnEnemy(enemyConfig.enemyPrefab, enemyConfig.enemySpeed);
                yield return new WaitForSeconds(enemyConfig.spawnRate);
            }

            yield return new WaitUntil(() => _activeEnemies.Count == 0);

            _currentWave++;
        }

        Debug.Log("All waves completed.");
        LevelManager.Main?.ShowLevelCompleted();
    }

    private IEnumerable<EnemyConfig> CreateEnemyQueue(EnemyWaveConfig wave)
    {
        List<EnemyConfig> enemyQueue = new();
        foreach (var enemy in wave.enemies)
        {
            for (int i = 0; i < enemy.enemyCount; i++)
            {
                enemyQueue.Add(enemy);
            }
        }

        ShuffleList(enemyQueue);
        return enemyQueue;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void SpawnEnemy(GameObject prefab, float speed)
    {
        if (prefab == null || LevelManager.Main?.StartPoint == null) return;

        GameObject enemy = Instantiate(prefab, LevelManager.Main.StartPoint.position, Quaternion.identity);
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.enemySpawner = this;
            movement.SetSpeed(speed);
            _activeEnemies.Add(movement);
        }
    }

    public void EnemyDestroyed(EnemyMovement enemy)
    {
        if (!_activeEnemies.Remove(enemy)) return;

        if (enemy.IsRewarded)
        {
            Debug.LogWarning("Enemy already rewarded.");
            return;
        }

        int reward = enemy.GetReward();
        CurrencyController.Instance?.IncreaseCurrency(reward);
        StatisticsManager.Instance?.IncrementEnemiesKilled(1);

        Debug.Log($"Enemy destroyed. Remaining enemies: {_activeEnemies.Count}");
    }

    public void ResetSpawnerForGameOver()
    {
        StopAllCoroutines();

        foreach (var enemy in _activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        _activeEnemies.Clear();

        _currentWave = 0;

        Debug.Log("EnemySpawner: Spawner reset for Game Over.");
    }
}
