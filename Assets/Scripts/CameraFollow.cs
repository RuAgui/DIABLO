using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;

    Vector3 offset;
    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, speed * Time.deltaTime);
    }
}
