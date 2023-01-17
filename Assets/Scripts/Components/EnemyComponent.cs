using System.Linq;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public int startHealth = 2;
    private int health;

    public bool IsDied => health <= 0;

    public HealthbarComponent healthbar;

    public Animator animator;

    public Rigidbody[] ragdollRigidbodies;

    private void Start()
    {
        health = startHealth;

        SwitchRagdoll(false);
    }

    public void Initialize(PlayerComponent player)
    {
        healthbar.Initialize(player.playerCamera);
        healthbar.UpdateHealth(startHealth, health);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        healthbar.UpdateHealth(startHealth, health);
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        SwitchRagdoll(true);

        var hitRigidbody = ragdollRigidbodies.OrderBy((body) => Vector3.SqrMagnitude(body.position - hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint);
    }

    private void SwitchRagdoll(bool enableRagdoll)
    {
        animator.enabled = !enableRagdoll;

        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = !enableRagdoll;
        }
    }
}
