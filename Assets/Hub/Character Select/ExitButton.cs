using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour, ICursorButtonable
{
    public bool stageSelect;
    public GameObject transitioner;
    public PlayerNumberCharacterSelect playerNumberCharacterSelect;    

    private TokenControl[] tokens;
    private CharacterSelectControl[] hands;
    
    
    public void HoverBegin()
    {

    }

    public void HoverStay()
    {

    }

    public void HoverExit()
    {

    }

    public void Click()
    {
        // Exit back to previous state
        transitioner.GetComponent<ITransitionable>().Transition();
        
        // Clear hands
        playerNumberCharacterSelect.ClearAll();
        DestroyHands();
        
    }


    // Destroys all the hands
    private void DestroyHands()
    {
        tokens = (TokenControl[])FindObjectsOfType(typeof(TokenControl));
        hands = (CharacterSelectControl[])FindObjectsOfType(typeof(CharacterSelectControl));
        if (tokens != null)
        {
            // Destroy them
            for (int i = 0; i < tokens.Length; i++)
            {
                Destroy(tokens[i].gameObject);
            }
            for (int i = 0; i < hands.Length; i++){
                Destroy(hands[i].gameObject);
            }
        }
    }
}
