using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class CharToStageSelect : MonoBehaviour, ITransitionable
{
    public PlayableDirector stageToChar;
    public PlayableDirector charToStage;
    public GameObject stageSelectCursorPrefab;
    private TokenControl[] tokens;
    private CharacterSelectControl[] hands;
    private SmashSettings playerCharList;
    private PlayerInput cursorPlayerInput;
    private PodiumControl[] podiums;


    // Start is called before the first frame update
    void Start()
    {
        playerCharList = GameObject.FindWithTag("SmashSettings").GetComponent<SmashSettings>();
        podiums = (PodiumControl[])FindObjectsOfType(typeof(PodiumControl));
    }

    public void Transition(bool forward = false)
    {
        if (forward)
        {
            tokens = (TokenControl[])FindObjectsOfType(typeof(TokenControl));
            hands = (CharacterSelectControl[])FindObjectsOfType(typeof(CharacterSelectControl));
            foreach(CharacterSelectControl hand in hands)
            {
                hand.Freeze();
            }
            // Instantiate the stage select cursor, assigning all input devices to control it
            cursorPlayerInput = PlayerInput.Instantiate(stageSelectCursorPrefab, -1, null, -1, playerCharList.GetDeviceList().ToArray());
            // Transition
            stageToChar.Stop();
            charToStage.Play();
        }
        else
        {
            // Remove the shared stage select cursor
            Destroy(cursorPlayerInput.gameObject);
            // Transition
            charToStage.Stop();
            stageToChar.Play();
            // Activate all the hands
            playerCharList.RecreateExitedPlayers();
            // Reset the podiums
            foreach(PodiumControl podium in podiums)
            {
                podium.ResetPosition();
            }
        }
    }
}
