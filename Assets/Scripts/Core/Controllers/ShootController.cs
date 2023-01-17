using UnityEngine;

public class ShootController
{
    private readonly PlayerComponent player;
    private readonly ShootingSystem manager;

    private const float _maxRayDistance = 5f;

    public ShootController(PlayerComponent player, ShootingSystem manager)
    {
        this.player = player;
        this.manager = manager;
    }

    public void Shoot(Vector3 position)
    {
        var ray = player.playerCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out var hit))
        {
            manager.SpawnBullet(player.bulletSpawnPoint.position, hit.point - player.bulletSpawnPoint.position);
        }
        else
        {
            var direction = ray.direction * _maxRayDistance + player.playerCamera.transform.position - player.bulletSpawnPoint.position;
            manager.SpawnBullet(player.bulletSpawnPoint.position, direction);
        }
    }
}
