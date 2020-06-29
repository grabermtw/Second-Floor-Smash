using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class FPSToCharSelect : HubInteractor
{
    public PlayableDirector fpsToChar;
    public PlayableDirector charToFps;
    public PlayerInputManager charSelectInput;
    public PlayerInputManager fpInput;

    void Update()
    {
        base.UpdateStatus();
    }

    public override void Interact()
    {
        Transition(true);
    }

    public void Transition(bool toChar = false)
    {
        if(toChar)
        {
            charToFps.Stop();
            fpsToChar.Play();
            fpInput.enabled = false;
            charSelectInput.enabled = true;
            charToFps.time = 0;
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
