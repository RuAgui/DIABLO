using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Rigidbody fireBallPrefabs;
    [SerializeField] Transform shootPoint;
    [SerializeField] private float shootForce;

    private Animator anim;
    private PlayerMovement playerMovement;
    private NavMeshAgent agent;
    private Ray ray;
    private RaycastHit hit;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Attack();
        }
    }

    private void Attack()
    {
        anim.Play("Attack");
        playerMovement.canMove = false;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
        Turn();
    }

    private void Shoot()
    {
        Rigidbody cloneFireBallPrefab = Instantiate(fireBallPrefabs, shootPoint.position, shootPoint.rotation);
        cloneFireBallPrefab.AddForce(shootPoint.forward * shootForce);

        Destroy(cloneFireBallPrefab.gameObject, 4);
    }
    private void Turn()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }
}
