using Unity.Netcode;
using UnityEngine;

public class Connect : MonoBehaviour
{
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
    }
}
