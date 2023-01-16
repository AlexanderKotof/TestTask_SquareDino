using System;
using System.Linq;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public int health = 2;
    public bool IsDied => health <= 0;

    public Animator animator;

    public Rigidbody[] ragdollRigidbodies;

    private void Start()
    {
        animator.enabled = true;
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
    }


    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        animator.enabled = false;

        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        var hitRigidbody = ragdollRigidbodies.OrderBy((body) => Vector3.SqrMagnitude(body.position - hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint);
    }

}
