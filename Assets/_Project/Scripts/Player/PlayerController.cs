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
            float offset = 0.5f; // Ajuste o valor do offset conforme necessário
            // Cria um ponto de origem para o projétil
            Vector2 projectileOrigin =
                (Vector2)projectileSpawnPoint.position + offset * Vector2.right;
            // Calcula a direção do mouse em relação ao ponto de origem
            Vector2 direction = mousePosition - projectileOrigin;
            // Normaliza a direção
            direction.Normalize();
            // Calcula a rotação necessária para o projétil
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
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
            .AddForce(rotation * Vector2.right * projectileSpeed, ForceMode2D.Impulse);
    }
}
