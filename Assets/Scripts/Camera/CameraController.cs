using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 5f;
    public float lookingForwardDistance = 1.0f;
    public Transform target;
    private PlayerController playerController;

    void Start()
    {
        if (target != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            // Looking forward
            transform.position = Vector3.Lerp(transform.position, targetPosition + (Vector3)playerController.lookDirection * lookingForwardDistance, followSpeed * Time.deltaTime);
        }

    }
}
