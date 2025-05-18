using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject pistolPrefab;

    [SerializeField]
    private GameObject sniperPrefab;

    [SerializeField]
    private GameObject swordPrefab;

    private void Awake()
    {
        Debug.Log("Awake - NetworkPlayerSpawner");
    }

    void Start()
    {
        int gameMode = PlayerPrefs.GetInt("GameMode", 0);
        Debug.Log("Game mode: " + gameMode);
        if (gameMode == 1)
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Game is set as host");
        }
        else if (gameMode == 2)
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Game is set as client");
        }
        else
        {
            Debug.Log("Game mode not set. Please select a game mode.");
            return;
        }
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn - NetworkPlayerSpawner");
        SpawnPlayerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong senderClientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"SpawnPlayerServerRpc - clientId: {senderClientId}");

        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", -1);
        if (selectedCharacter == -1)
        {
            Debug.Log("No character selected. Please select a character.");
            return;
        }

        SpawnPlayerOnClients(senderClientId, selectedCharacter);
    }

    void SpawnPlayerOnClients(ulong clientId, int selectedCharacter)
    {
        Debug.Log($"SpawnPlayerOnClients - clientId: {clientId}");

        GameObject playerPrefab = null;
        switch (selectedCharacter)
        {
            case 0:
                playerPrefab = swordPrefab;
                break;
            case 1:
                playerPrefab = pistolPrefab;
                break;
            case 2:
                playerPrefab = sniperPrefab;
                break;
            default:
                Debug.LogError("Invalid character selection.");
                return;
        }

        GameObject player = Instantiate(playerPrefab);
        var playerNetworkObject = player.GetComponent<NetworkObject>();
        playerNetworkObject.SpawnAsPlayerObject(clientId);
    }
}
