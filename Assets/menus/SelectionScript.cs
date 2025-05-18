using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    
    [SerializeField] private GameObject[] characterCards;
    
    public void SelectCharacter(int characterIndex)
    {
        if(characterIndex == -1)
        {
            foreach (GameObject card in characterCards)
            {
                card.SetActive(true);
            }
            return;
        }
        
        foreach (GameObject card in characterCards)
        {
            card.SetActive(false);
        }

        // Show the selected character card
        if (characterIndex >= 0 && characterIndex < characterCards.Length)
        {
            characterCards[characterIndex].SetActive(true);
        }
        
        //print the selected character index
        Debug.Log("Selected character index: " + characterIndex);
    }
    
    
}
