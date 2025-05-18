using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField roomAddressInput;

    public void SetAsHost()
    {
        PlayerPrefs.SetInt("GameMode", 1);
        PlayerPrefs.Save();
        Debug.Log("Game mode set to host");
    }

    public void SetAsClient()
    {
        PlayerPrefs.SetInt("GameMode", 2);
        PlayerPrefs.SetString("HostAddress", roomAddressInput.text);
        PlayerPrefs.Save();
        Debug.Log("Game mode set to client");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }
}
