using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead;
    [SerializeField] private float maxHealth;
    [SerializeField] Image healthImage;

    private float currentHealth;
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageEnemy"))
        {
            currentHealth--;
            healthImage.fillAmount = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }
    void Death()
    {
        isDead = true;
        playerAttack.enabled = false;
        playerMovement.enabled = false;
        anim.Play("Death");
    }
}
