using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToSelectCharacter ()
    {
        //go to the select character menu
    }
    
    public void GoToOptions ()
    {
        //go to the options menu
        
    }

    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("Game is quitting");
    }
   
}
