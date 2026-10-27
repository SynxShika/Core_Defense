using UnityEngine;

public class WalkerSpawner : EnemySpawner
{
    [SerializeField] private Walker walkerPrefab;

    public override Enemy SpawnEnemy(Vector2 position)
    {
        return Instantiate(walkerPrefab, position, Quaternion.identity);
    }
}
