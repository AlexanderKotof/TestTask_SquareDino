using System;
using System.Collections;
using UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerComponent playerPrefab;

    public ShootingManager shootingManager;
    public PlayerComponent Player { get; private set; }

    private WayPoints _wayPoints;

    private int _currentWayPointIndex = 0;

    private const float _requiredDistance = 0.1f;
    private const float _maxRayDistance = 5f;

    private const string _gameSceneName = "Game";

    private void Start()
    {
        DontDestroyOnLoad(this);
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        var loading = SceneManager.LoadSceneAsync(_gameSceneName);
        loading.completed += GameSceneLoaded;

        ScreenSystem.ScreensManager.ShowScreen<LoadingScreen>();
    }

    private void GameSceneLoaded(AsyncOperation obj)
    {
        obj.completed -= GameSceneLoaded;

        _wayPoints = SceneContext.GetWaypoints();
        InstatiatePlayer();

        ScreenSystem.ScreensManager.HideScreen<LoadingScreen>();
        ScreenSystem.ScreensManager.ShowScreen<StartScreen>().SetCallback(StartGame);
    }

    private void StartGame()
    {
        ScreenSystem.ScreensManager.HideScreen<StartScreen>();
        ScreenSystem.ScreensManager.ShowScreen<GameScreen>().SetShootCallback(Shoot);

        _currentWayPointIndex = 0;

        MoveToNext();
    }

    private void InstatiatePlayer()
    {
        var firstWayPointTransform = _wayPoints.points[0].transform;
        Player = Instantiate(playerPrefab, firstWayPointTransform.position, firstWayPointTransform.rotation);
    }

    private void Shoot(Vector3 position)
    {
        var ray = Player.playerCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out var hit, _maxRayDistance * 2))
        {
            shootingManager.SpawnBullet(Player.bulletSpawnPoint.position, hit.point - Player.bulletSpawnPoint.position);
        }
        else
        {
            var direction = ray.direction * _maxRayDistance + Player.playerCamera.transform.position - Player.bulletSpawnPoint.position;
            shootingManager.SpawnBullet(Player.bulletSpawnPoint.position, direction);
        }
    }

    private void MoveToNext()
    {
        _currentWayPointIndex++;

        if (_wayPoints.points.Length <= _currentWayPointIndex)
        {
            LevelEnded();
            return;
        }

        StartCoroutine(MoveToWayPoint(_currentWayPointIndex));
    }

    private void LevelEnded()
    {
        StopAllCoroutines();

        ScreenSystem.ScreensManager.HideScreen<GameScreen>();
        ScreenSystem.ScreensManager.ShowScreen<LevelEndScreen>().SetCallback(RestartGame);
    }

    private void RestartGame()
    {
        ScreenSystem.ScreensManager.HideScreen<LevelEndScreen>();
        LoadGameScene();
    }

    private IEnumerator MoveToWayPoint(int index)
    {
        var wayPoint = _wayPoints.points[index];
        Player.MoveToPosition(wayPoint.transform.position);

        while ((Player.transform.position - wayPoint.transform.position).sqrMagnitude > _requiredDistance * _requiredDistance)
        {
            yield return null;
        }

        WayPointReached(wayPoint);
    }

    private IEnumerator WaitForEnemiesDie(WayPointComponent wayPoint)
    {
        while (true)
        {
            bool wayPointCleared = true;
            foreach (var enemies in wayPoint.wayPointEnemySpawns)
            {
                if (!enemies.SpawnedEnemy.IsDied)
                {
                    wayPointCleared = false;
                    break;
                }
            }

            if (wayPointCleared)
                break;

            yield return null;
        }

        MoveToNext();
    }

    private void WayPointReached(WayPointComponent wayPoint)
    {
        if (wayPoint.wayPointEnemySpawns.Length > 0)
        {
            StartCoroutine(WaitForEnemiesDie(wayPoint));
            return;
        }

        MoveToNext();
    }
}
