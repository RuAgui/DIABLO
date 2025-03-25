using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] checkPoints;

    [Header("Alert")]
    [SerializeField] private float visionRange;
    [SerializeField] private float visionAngle;
    [SerializeField] public bool playerDetected;

    [Header("Speed")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float attackingSpeed;

    [Header("Attack")]
    [SerializeField] Collider colliderAttack;

    private int index; //Para saber en que punto de la patrulla estamos

    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    private PlayerHealth playerHealth;


    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        PatrolAndAlert(true);

    }

    void Update()
    {
        Animating();
    }

    void Animating()
    {
        anim.SetFloat("Velocity", agent.velocity.magnitude);
    }

    public void PatrolAndAlert(bool state)
    {
        if (state)
        {
            StartCoroutine(nameof(Patrol));
            StartCoroutine(nameof(Alert));
        }
    }

    public void Attacking (bool state)
    {
        if (state)
        {
            StartCoroutine(nameof(Attack));
        }
        else
        {
            StopCoroutine(nameof(Attack));
        }
    }

    IEnumerator Patrol()
    {
            agent.speed = patrolSpeed;
        #region Patrol by Checkpoints
        while (true)
        {
            agent.SetDestination(checkPoints[index].position);
            //Mientrar no hayas llegado a tu destino, no hagas nada
            while (Vector3.Distance(transform.position, checkPoints[index].position) > agent.stoppingDistance)
            {
                yield return null;
            }
            //Escoger nuevo punto en la patrulla
            yield return new WaitForSeconds(Random.Range(1, 3)); //Hago un tiempo de espera aleatorio
            index++;
            if (index >= checkPoints.Length)
            {
                index = 0;
            }
        }
        #endregion
        #region Patrol by Random
        //    Vector3 destination = transform.position;
        //while (true)
        //{
        //    while (Vector3.Distance(transform.position, destination) > agent.stoppingDistance)
        //    {
        //        yield return null;
        //        if (agent.velocity.magnitude <= 0.1f)
        //        {
        //            destination = transform.position;
        //        }
        //    }
        //    //Me espero
        //    yield return new WaitForSeconds(Random.Range(1, 3));
        //    //Cojo punto aleatorio dentro de una esfera fija
        //    Vector3 randomPoint = checkPoints[0].position + (Random.insideUnitSphere * 4);
        //    NavMeshHit hit;
        //    NavMesh.SamplePosition(randomPoint, out hit, 10, 1);
        //    destination = hit.position;
        //    agent.SetDestination(destination);
        //}
        #endregion

    }

    IEnumerator Alert()
    {
        while (true)
        {
            Vector3 direction = player.position - transform.position;
            float distance = Vector3.Distance(transform.position, player.position);
            float angle = Vector3.Angle(transform.forward, direction);
            float diffy = Mathf.Abs(transform.position.y - player.position.y);

            //La condicion para detectar al player:

            if (distance < visionRange && angle < visionAngle && diffy < 0.5f)
            {
                playerDetected = true;
                PatrolAndAlert(false);
                Attacking(true);
            }
            yield return null;
        }  
    }

    IEnumerator Attack()
    {
        Debug.Log("Q TE MATO!");
        playerDetected = true;
        agent.speed = attackingSpeed;
        agent.ResetPath();
        anim.Play("Attack");

        yield return new WaitForSeconds(2);

        while (true)
        {
            //Si el playyer esta demasiado lejos para atacarle, voy a por el o se escapa.
            while (Vector3.Distance(transform.position, player.position) > agent.stoppingDistance)
            {
                //si el player se escapa de mi rango
               if(Vector3.Distance(transform.position, player.position) > visionRange * 2)
                {
                    //Vuelvo a patrullar
                    PatrolAndAlert(true);
                    Attacking(false);
                    playerDetected = false;
                }
                else
                {
                    agent.SetDestination(player.position);
                }
                yield return null;
            }

            if (!playerHealth.isDead)
            {
                //sino se cumple el while, significa que estoy cerca para atacar
                transform.LookAt(player.position);
                anim.Play("Attack");
                yield return new WaitForSeconds(2); 
            }
            
        }
    }

    void ActivateCollider()
    {
        colliderAttack.enabled = true;
    }

    void DeactivateCollider()
    {
        colliderAttack.enabled = false;
    }

    
    private void OnDrawGizmos()
    {
        //Dibujo una esfera con el rango de vision del enemigo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.green;
        Vector3 righDir = Quaternion.Euler(0, visionAngle, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, -visionAngle, 0) * transform.forward;

        Gizmos.DrawRay(transform.position + new Vector3(0, 1, 0), righDir * visionRange);
        Gizmos.DrawRay(transform.position + new Vector3(0, 1, 0), leftDir * visionRange);
    }
}
