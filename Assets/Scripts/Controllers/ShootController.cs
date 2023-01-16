﻿using UnityEngine;

public class ShootController
{
    private readonly PlayerComponent player;
    private readonly ShootingManager manager;

    private const float _maxRayDistance = 5f;

    public ShootController(PlayerComponent player, ShootingManager manager)
    {
        this.player = player;
        this.manager = manager;
    }

    public void Shoot(Vector3 position)
    {
        var ray = player.playerCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out var hit, _maxRayDistance * 2))
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