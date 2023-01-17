using System;
using System.Collections;

public class PlayerMovementSystem : IDisposable
{
    private PlayerComponent _player;
    private Action<WayPointComponent> _wayPointReached;

    private const float _distanceThreashold = 0.1f;

    public PlayerMovementSystem(PlayerComponent player, Action<WayPointComponent> wayPointReachedCalllback)
    {
        _player = player;
        _wayPointReached = wayPointReachedCalllback;
    }

    public void MoveToWaypoint(WayPointComponent wayPointComponent)
    {
        _player.StartCoroutine(MoveToWayPoint(wayPointComponent));
    }

    private IEnumerator MoveToWayPoint(WayPointComponent wayPoint)
    {
        _player.MoveToPosition(wayPoint.transform.position);

        while ((_player.transform.position - wayPoint.transform.position).sqrMagnitude > _distanceThreashold * _distanceThreashold)
        {
            yield return null;
        }

        _wayPointReached(wayPoint);
    }

    public void Dispose()
    {
        _player = null;
        _wayPointReached = null;
    }
}
