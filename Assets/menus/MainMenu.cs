using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void SetAsHost()
    {
        PlayerPrefs.SetInt("GameMode", 1);
        Debug.Log("Game mode set to host");
    }

    public void SetAsClient()
    {
        PlayerPrefs.SetInt("GameMode", 2);
        Debug.Log("Game mode set to client");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }
}
