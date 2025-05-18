using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float rotationSpeed = 720f;

    [SerializeField]
    private float positionRange = 3f;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.SetPositionAndRotation(
                new Vector2(
                    Random.Range(-positionRange, positionRange),
                    Random.Range(-positionRange, positionRange)
                ),
                Quaternion.Euler(0, 0, Random.Range(0f, 360f))
            );
        }
    }

    void Update()
    {
        if (!IsOwner)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

        transform.Translate(movementDirection * speed * Time.deltaTime);

        if (movementDirection != Vector2.zero)
        {
            float targetAngle =
                Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(
                transform.eulerAngles.z,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePositionServerRpc()
    {
        Vector2 newPosition = transform.position;
        transform.position = newPosition;
    }
}
