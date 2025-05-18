using Unity.Netcode;
using UnityEngine;

public class CameraSetup : NetworkBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // Desativa a câmera de outros players
            playerCamera.gameObject.SetActive(false);
        }
        else
        {
            // Ativa a câmera do dono
            playerCamera.gameObject.SetActive(true);
        }
    }
}
