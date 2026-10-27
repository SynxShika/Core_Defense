using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    public abstract Enemy SpawnEnemy(Vector2 position);
}
