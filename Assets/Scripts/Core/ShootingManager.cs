using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public BulletComponent bulletPrefab;

    private List<BulletComponent> _bulletPool;

    private const float _forceMultiplier = 100;
    private const int _prespawnCount = 5;

    private void Start()
    {
        _bulletPool = new List<BulletComponent>(_prespawnCount);

        for (int i = 0; i < _prespawnCount; i++)
        {
            AddBullet();
        }

        BulletComponent.HitEnemy += OnHitEnemy;
    }

    private void OnDestroy()
    {
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

    private BulletComponent AddBullet()
    {
        var bullet = Instantiate(bulletPrefab, transform);
        bullet.gameObject.SetActive(false);
        _bulletPool.Add(bullet);
        return bullet;
    }

    public void SpawnBullet(Vector3 position, Vector3 direction)
    {
        foreach(var bullet in _bulletPool)
        {
            if (bullet.gameObject.activeSelf)
                continue;

            bullet.Shoot(position, direction);
            return;
        }

        AddBullet().Shoot(position, direction);
    }

    
}
