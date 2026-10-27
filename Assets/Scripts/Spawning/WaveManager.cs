using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    private class WaveEntry
    {
        public EnemySpawner spawner;
        public int firstWave = 1;
        public int baseCount = 3;
        public float extraPerWave = 1f;

        public int GetCount(int wave)
        {
            if (wave < firstWave) return 0;
            return baseCount + Mathf.FloorToInt(extraPerWave * (wave - firstWave));
        }
    }

    [SerializeField] private List<WaveEntry> waveEntries = new List<WaveEntry>();
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 0.8f;
    [SerializeField] private float minSpawnInterval = 0.25f;
    [SerializeField] private float intervalReductionPerWave = 0.05f;

    private readonly List<Enemy> _aliveEnemies = new List<Enemy>();
    private bool _isSpawning;
    private Coroutine _waveRoutine;

    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        if (state != GameState.Defend) return;

        if (_waveRoutine != null) StopCoroutine(_waveRoutine);
        _waveRoutine = StartCoroutine(RunWave(GameManager.Instance.Wave));
    }

    private IEnumerator RunWave(int wave)
    {
        _isSpawning = true;

        List<EnemySpawner> queue = BuildSpawnQueue(wave);
        float interval = Mathf.Max(minSpawnInterval, spawnInterval - intervalReductionPerWave * (wave - 1));

        foreach (EnemySpawner spawner in queue)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Enemy enemy = spawner.SpawnEnemy(point.position);

            _aliveEnemies.Add(enemy);
            enemy.OnRemoved += HandleEnemyRemoved;

            yield return new WaitForSeconds(interval);
        }

        _isSpawning = false;
        CheckWaveComplete();
    }

    private List<EnemySpawner> BuildSpawnQueue(int wave)
    {
        List<EnemySpawner> queue = new List<EnemySpawner>();

        foreach (WaveEntry entry in waveEntries)
        {
            int count = entry.GetCount(wave);
            for (int i = 0; i < count; i++) queue.Add(entry.spawner);
        }

        for (int i = queue.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (queue[i], queue[j]) = (queue[j], queue[i]);
        }

        return queue;
    }

    private void HandleEnemyRemoved(Enemy enemy)
    {
        enemy.OnRemoved -= HandleEnemyRemoved;
        _aliveEnemies.Remove(enemy);
        CheckWaveComplete();
    }

    private void CheckWaveComplete()
    {
        if (_isSpawning || _aliveEnemies.Count > 0) return;
        if (GameManager.Instance.State == GameState.Defend)
            GameManager.Instance.CompleteWave();
    }
}
