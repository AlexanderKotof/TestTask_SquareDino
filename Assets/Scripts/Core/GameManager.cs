using ScreenSystem;
using System.Collections;
using UI.Screens;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerComponent playerPrefab;
    public BulletComponent bulletPrefab;
    public PlayerComponent Player { get; private set; }

    private ShootingSystem _shootingSystem;
    private PlayerMovementSystem _playerMovementSystem;
    private WayPoints _wayPoints;

    private int _currentWayPointIndex = 0;

    private void Start()
    {
        DontDestroyOnLoad(this);
        GameSceneLoader.LoadGameScene(GameSceneLoaded);
    }

    private void GameSceneLoaded()
    {
        _wayPoints = SceneContext.GetWaypoints();
        InstatiatePlayer();
        ScreensManager.ShowScreen<StartScreen>().SetCallback(StartGame);
    }

    private void StartGame()
    {
        ScreensManager.HideScreen<StartScreen>();

        _shootingSystem = new ShootingSystem(bulletPrefab, transform);
        _playerMovementSystem = new PlayerMovementSystem(Player, OnWayPointReached);

        _currentWayPointIndex = 0;

        ScreensManager.ShowScreen<GameScreen>().SetController(new ShootController(Player, _shootingSystem));

        MoveToNext();
    }

    private void InstatiatePlayer()
    {
        var firstWayPointTransform = _wayPoints.points[0].transform;
        Player = Instantiate(playerPrefab, firstWayPointTransform.position, firstWayPointTransform.rotation);
    }

    private void MoveToNext()
    {
        _currentWayPointIndex++;

        if (_wayPoints.points.Length <= _currentWayPointIndex)
        {
            LevelEnded();
            return;
        }

        var point = _wayPoints.points[_currentWayPointIndex];
        _playerMovementSystem.MoveToWaypoint(point);
    }

    private void LevelEnded()
    {
        _shootingSystem.Dispose();
        _playerMovementSystem.Dispose();

        ScreensManager.HideScreen<GameScreen>();
        ScreensManager.ShowScreen<LevelEndScreen>().SetCallback(RestartGame);
    }

    private void RestartGame()
    {
        ScreensManager.HideScreen<LevelEndScreen>();
        GameSceneLoader.LoadGameScene(GameSceneLoaded);
    }

    private IEnumerator WaitForEnemiesDie(WayPointComponent wayPoint)
    {
        foreach (var enemy in wayPoint.wayPointEnemySpawns)
        {
            enemy.SpawnedEnemy.Initialize(Player);
        }

        while (true)
        {
            bool wayPointCleared = true;
            foreach (var enemy in wayPoint.wayPointEnemySpawns)
            {
                if (!enemy.SpawnedEnemy.IsDied)
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

    private void OnWayPointReached(WayPointComponent wayPoint)
    {
        if (wayPoint.wayPointEnemySpawns.Length > 0)
        {
            StartCoroutine(WaitForEnemiesDie(wayPoint));
            return;
        }

        MoveToNext();
    }
}
