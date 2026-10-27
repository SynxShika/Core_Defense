using UnityEngine;

public class BruteSpawner : EnemySpawner
{
    [SerializeField] private Brute brutePrefab;

    public override Enemy SpawnEnemy(Vector2 position)
    {
        return Instantiate(brutePrefab, position, Quaternion.identity);
    }
}
