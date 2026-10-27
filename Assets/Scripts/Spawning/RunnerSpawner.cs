using UnityEngine;

public class RunnerSpawner : EnemySpawner
{
    [SerializeField] private Runner runnerPrefab;

    public override Enemy SpawnEnemy(Vector2 position)
    {
        return Instantiate(runnerPrefab, position, Quaternion.identity);
    }
}
