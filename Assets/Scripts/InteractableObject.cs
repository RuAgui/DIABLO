using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] Collider trigger;
    public bool equippable;
    [SerializeField] Vector3 offsetEquip;

    Animator anim;
    Animator animButton;
    PlayerActions playerActions;
    Transform pivotWeapon;

    void Awake()
    {
        anim = GetComponent<Animator>();
        animButton = button.GetComponentInParent<Animator>();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
        pivotWeapon = playerActions.pivotWeapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange(true, this);  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange(false, null);
        }
    }

    void PlayerInRange(bool state, InteractableObject _object)
    {
        button.SetActive(state);
        animButton.SetBool("ShowUp", state);
        playerActions.interactableObject = _object;
        playerActions.canInteract = state;
    }

    public void Interact()
    {
        anim.SetTrigger("Interact");
        DesactivateButtonAndCollider(false);
        trigger.enabled = false;
        button.SetActive(false);
        //this.enabled = false;
        IsTheObjectAnObstacle();
    }

    void IsTheObjectAnObstacle()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle)
        {
            obstacle.enabled = false;
        }
    }
    public void Equip()
    {
        DesactivateButtonAndCollider(false);
        transform.SetParent(pivotWeapon);
        transform.localPosition = offsetEquip;
        transform.localRotation = Quaternion.identity;
    }

    public void DesactivateButtonAndCollider(bool state)
    {
        button.SetActive(state);
        button.SetActive(state);
    }
}
