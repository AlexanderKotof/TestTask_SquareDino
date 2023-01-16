using UnityEngine;

public class SpawnPointComponent : MonoBehaviour
{
    [SerializeField]
    private EnemyComponent enemyPrefab;
    public EnemyComponent SpawnedEnemy { get; private set; }

    private void Awake()
    {
        SpawnedEnemy = Instantiate(enemyPrefab, transform);
    }
}
