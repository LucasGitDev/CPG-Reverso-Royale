using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : NetworkBehaviour
{
    private NetworkVariable<FixedString128Bytes> networkPlayerName = new(
        new FixedString128Bytes("Player 0"),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        networkPlayerName.Value = "Player " + (OwnerClientId + 1);
        // playerNameText.text = networkPlayerName.Value.ToString();
    }
}
