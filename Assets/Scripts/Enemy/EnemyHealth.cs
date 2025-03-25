using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider slider;

    private float currentHealth;
    private NavMeshAgent agent;
    private Animator anim;
    private Enemy enemy;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FireBall"))
        {
            if (enemy.playerDetected == false)
            {
                enemy.Attacking(true);
                enemy.PatrolAndAlert(false);
            }
            currentHealth--;
            slider.value = currentHealth;
            Destroy(collision.gameObject);
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }
    private void Death()
    {
        enemy.Attacking(false);
        enemy.PatrolAndAlert(false);
    }
}

