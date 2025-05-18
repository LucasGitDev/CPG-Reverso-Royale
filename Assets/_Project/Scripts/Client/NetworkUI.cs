using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button clientButton;

    [SerializeField]
    private TextMeshProUGUI playersCountText;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone
    );

    // Update is called once per frame
    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    public void Update()
    {
        playersCountText.text = $"Players: {playersCount.Value}";

        if (!IsServer)
            return;

        playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
