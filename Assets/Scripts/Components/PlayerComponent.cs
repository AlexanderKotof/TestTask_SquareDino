using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerComponent : MonoBehaviour
{
    public Camera playerCamera;
    public NavMeshAgent agent;
    public Transform bulletSpawnPoint;
    public Animator animator;

    private const float _movingVelocityThreashold = 0.1f;

    public void MoveToPosition(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void Update()
    {
        bool isMoving = agent.velocity.sqrMagnitude > _movingVelocityThreashold;
        animator.SetBool("IsMoving", isMoving);
    }
}
