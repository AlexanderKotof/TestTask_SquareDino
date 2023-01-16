using System.Collections;
using UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerComponent playerPrefab;
    public ShootingManager shootingManager;

    public PlayerComponent Player { get; private set; }

    private PlayerMovementSystem _playerMovementSystem;
    private WayPoints _wayPoints;

    private int _currentWayPointIndex = 0;

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
        ScreenSystem.ScreensManager.ShowScreen<GameScreen>().SetController(new ShootController(Player, shootingManager));

        _playerMovementSystem = new PlayerMovementSystem(Player, OnWayPointReached);

        _currentWayPointIndex = 0;

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
        StopAllCoroutines();

        ScreenSystem.ScreensManager.HideScreen<GameScreen>();
        ScreenSystem.ScreensManager.ShowScreen<LevelEndScreen>().SetCallback(RestartGame);
    }

    private void RestartGame()
    {
        ScreenSystem.ScreensManager.HideScreen<LevelEndScreen>();
        LoadGameScene();
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
