using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class FPSToCharSelect : HubInteractor, ITransitionable
{
    public PlayableDirector fpsToChar;
    public PlayableDirector charToFps;
    public PlayerInputManager charSelectInput;
    public PlayerInputManager fpInput;
    public PlayerCharList playerCharList;
    private PodiumControl[] podiums;
    
    private bool exitedOnceAlready = false;

    void Start()
    {
        podiums = (PodiumControl[])FindObjectsOfType(typeof(PodiumControl));
    }

    void Update()
    {
        base.UpdateStatus();
    }

    public override void Interact()
    {
        Transition(true);
    }

    public void Transition(bool forward = false)
    {
        if (forward)
        {
            foreach(PodiumControl podium in podiums)
            {
                podium.ResetPosition();
            }
            charToFps.Stop();
            fpsToChar.Play();
            fpInput.enabled = false;
            charSelectInput.enabled = true;
            charToFps.time = 0;
            if (exitedOnceAlready)
            {
                playerCharList.RecreateExitedPlayers();
            }
            exitedOnceAlready = true;
        }
        else
        {
            fpsToChar.Stop();
            charToFps.Play();
            charSelectInput.enabled = false;
            fpInput.enabled = true;
            fpsToChar.time = 0;
        }
    }

}
