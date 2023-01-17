using System;
using UnityEngine;

public class ShootingSystem : IDisposable
{
    private const float _forceMultiplier = 100;
    private const int _prespawnCount = 5;

    private ObjectPool<BulletComponent> _bulletPool;

    public ShootingSystem(BulletComponent bulletPrefab, Transform parent)
    {
        _bulletPool = new ObjectPool<BulletComponent>(bulletPrefab, parent, _prespawnCount);

        BulletComponent.HitEnemy += OnHitEnemy;
    }
    public void Dispose()
    {
        _bulletPool.Dispose();

        BulletComponent.HitEnemy -= OnHitEnemy;
    }

    private void OnHitEnemy(EnemyComponent enemy, BulletComponent bullet)
    {
        enemy.TakeDamage(bullet.damage);

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
