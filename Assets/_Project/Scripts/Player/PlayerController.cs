using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float projectileSpeed = 10f;

    [SerializeField]
    private Transform projectileSpawnPoint;

    void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rotation = Quaternion.LookRotation(
                Vector3.forward,
                mousePosition - (Vector2)projectileSpawnPoint.position
            );
            ShootProjectileServerRpc(rotation);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootProjectileServerRpc(Quaternion rotation)
    {
        // Chama o método ClientRpc para todos os clientes
        ShootProjectileClientRpc(rotation);
    }

    [ClientRpc]
    void ShootProjectileClientRpc(Quaternion rotation)
    {
        // Use o projectileSpawnPoint para a posição do projétil
        GameObject projectile = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            rotation
        );
        projectile.GetComponent<NetworkObject>().Spawn();
        projectile
            .GetComponent<Rigidbody2D>()
            .AddForce(rotation * Vector2.up * projectileSpeed, ForceMode2D.Impulse);
    }
}
