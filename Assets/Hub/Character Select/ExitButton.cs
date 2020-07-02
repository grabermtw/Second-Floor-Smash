using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour, ICursorButtonable
{
    public FPSToCharSelect transitioner;
    public PlayerNumberCharacterSelect playerNumberCharacterSelect;

    private TokenControl[] tokens;
    private CharacterSelectControl[] hands;
    

    public void Hover()
    {

    }

    public void Click()
    {
        // Exit back to the second floor
        transitioner.Transition();
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
