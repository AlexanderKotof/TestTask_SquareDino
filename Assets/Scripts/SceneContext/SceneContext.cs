using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    private static SceneContext _instance;

    public WayPoints wayPoints;

    private void Awake()
    {
        _instance = this;
    }

    public static WayPoints GetWaypoints()
    {
        return _instance.wayPoints;
    }
}
