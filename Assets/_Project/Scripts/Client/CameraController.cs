using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = new Vector3(
                player.position.x,
                player.position.y,
                transform.position.z
            );
            transform.position = newPosition;
        }
    }
}
