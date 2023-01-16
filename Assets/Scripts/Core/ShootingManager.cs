using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public BulletComponent bulletPrefab;

    private const float _forceMultiplier = 100;
    private const int _prespawnCount = 5;

    private ObjectPool<BulletComponent> _bulletPool;

    private void Start()
    {
        _bulletPool = new ObjectPool<BulletComponent>(bulletPrefab, transform, _prespawnCount);

        BulletComponent.HitEnemy += OnHitEnemy;
    }

    private void OnDestroy()
    {
        _bulletPool.Dispose();

        BulletComponent.HitEnemy -= OnHitEnemy;
    }

    private void OnHitEnemy(EnemyComponent enemy, BulletComponent bullet)
    {
        enemy.TakeDamage(1);

        if (enemy.IsDied)
        {
            var force = (bullet.Direction + Vector3.up).normalized * _forceMultiplier;
            enemy.TriggerRagdoll(force, bullet.transform.position);
        }
    }

    public void SpawnBullet(Vector3 position, Vector3 direction)
    {
        _bulletPool.Pool().Shoot(position, direction);
    }
}
