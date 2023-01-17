using UnityEngine;
using UnityEngine.UI;

public class HealthbarComponent : MonoBehaviour
{
    private Camera _mainCamera;
    public Slider healthbar;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(Camera camera)
    {
        gameObject.SetActive(true);
        _mainCamera = camera;
    }

    public void UpdateHealth(int startHealth, int health)
    {
        healthbar.maxValue = startHealth;
        healthbar.value = health;

        if (health <= 0)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_mainCamera.transform.position - transform.position);
    }
}
