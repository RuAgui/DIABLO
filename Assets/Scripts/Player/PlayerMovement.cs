using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove;

    [SerializeField] float turnSpeedThreshold = 0.5f;

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;

    Ray ray;
    RaycastHit hit;
    private bool move;
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        InputPlayer();
        Move();
        Animating();
        Rotate();
    }

    void InputPlayer()
    {
        if(Input.GetMouseButtonDown(0) && canMove) move = true;
    }
    void Move()
    {
        if (move)
        {
            move = false;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Lanzo raycast desde la camara
            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
    void Rotate()
    {
        //Debug.Log("Quaternion: " + transform.rotation);
        //Debug.Log("Euler: " + transform.eulerAngles);
        //transform.eulerAngles = new Vector3(0, -45, 0);
        if (agent.velocity.magnitude > turnSpeedThreshold)
        {
            transform.eulerAngles = new Vector3(0, Quaternion.LookRotation(agent.velocity).eulerAngles.y, 0);
        }
    }
    void Animating()
    {
        anim.SetFloat("Velocity", agent.velocity.magnitude);
    }

    public void ActiveMovement()
    {
        canMove = true;
    }
}
