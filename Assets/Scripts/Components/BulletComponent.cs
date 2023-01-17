using System;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public new Rigidbody rigidbody;

    public int damage = 1;

    public float speed = 5;

    public float lifeTime = 5f;

    public Vector3 Direction => _direction;

    private float _spawnTime;
    private Vector3 _direction;

    public static event Action<EnemyComponent, BulletComponent> HitEnemy;

    public void Shoot(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);
        _spawnTime = Time.realtimeSinceStartup;

        _direction = direction.normalized;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void Update()
    {
        rigidbody.velocity = speed * _direction;

        if (_spawnTime + lifeTime < Time.realtimeSinceStartup)
            Despawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyComponent>(out var enemyComponent))
        {
            HitEnemy?.Invoke(enemyComponent, this);
        }

        Despawn();
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}
