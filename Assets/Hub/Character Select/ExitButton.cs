using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour, ICursorButtonable
{
    public FPSToCharSelect transitioner;
    public PlayerCharList playerCharList;
    public PlayerNumberCharacterSelect playerNumberCharacterSelect;
    public void Hover()
    {

    }

    public void Click()
    {
        // Exit back to the second floor
        transitioner.Transition();
        // Clean it all up
        playerCharList.ClearAll();
        playerNumberCharacterSelect.ClearAll();
        TokenControl[] tokens = (TokenControl[])FindObjectsOfType(typeof(TokenControl));
        CharacterSelectControl[] hands = (CharacterSelectControl[])FindObjectsOfType(typeof(CharacterSelectControl));
        for(int i = 0; i < tokens.Length; i++)
        {
            Destroy(tokens[i].gameObject);
            Destroy(hands[i].gameObject);
        }
    }
}
