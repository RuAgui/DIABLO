using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool canInteract;
    public InteractableObject interactableObject; //Ref al objeto con el que interactuamos
    public Transform pivotWeapon;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            if (interactableObject.equippable) interactableObject.Equip();
            else interactableObject.Interact();
        }
        if (Input.GetKeyDown(KeyCode.F) && pivotWeapon.childCount > 0)
            DropWeapon();
    }

    void DropWeapon()
    {
        GameObject weapon = interactableObject.gameObject;
        weapon.transform.SetParent(null);

        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = transform.position + transform.forward + transform.up * 2;
        ray.direction = Vector3.down;

        if (Physics.Raycast(ray, out hit))
            weapon.transform.position = hit.point;
        else
            weapon.transform.position = transform.position + transform.forward;

        weapon.transform.rotation = Quaternion.identity;
        interactableObject.DesactivateButtonAndCollider(true);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
    }
}
