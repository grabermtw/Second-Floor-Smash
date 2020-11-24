using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : HubInteractor
{
    public enum State
    {
        Closed,
        Open,
        Closing,
        Opening,
    }

    public float maxAngleOpen = 90;
    public float flapTime = 3;
    public AudioClip[] openSounds;
    public AudioClip[] closeSounds;

    public State currentState;

    private Quaternion rotOpened; // Door's rotation when fully opened.
    private Quaternion rotClosed; // Door's rotation when full closed.

    private bool isFlapping = false; // Animate while true.

    // Set this according to whether we are going from zero
    // to one, or from one to zero.
    private float changeSign;

    private Transform hinge;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        hinge = transform.Find("Hinge"); // let's hope all the doors have hinges...
        audioSource = GetComponent<AudioSource>();
        rotOpened = Quaternion.Euler(0, maxAngleOpen, 0);
        rotClosed = Quaternion.Euler(0, 0, 0);
    }

    // Used by the player to open/close the door
    public override void Interact()
    {
        base.playerControl.UnsetInteract();
        StartCoroutine(FlapDoor());
    }

    // Used when an NPC wants to open the door
    public void OpenDoor()
    {
        if (currentState == State.Closed || currentState == State.Closing)
        {
            StartCoroutine(FlapDoor());
        }
    }

    // Used when an NPC wants to close the door
    public void CloseDoor()
    {
        if (currentState == State.Open || currentState == State.Opening)
        {
            StartCoroutine(FlapDoor());
        }
    }

    private void PlaySound(bool opening)
    {
        if (opening && openSounds.Length > 0)
        {
            audioSource.clip = openSounds[Random.Range(0, openSounds.Length)];
        }
        else if (closeSounds.Length > 0)
        {
            audioSource.clip = closeSounds[Random.Range(0, closeSounds.Length)];
        }
        audioSource.Play();
    }

    // Coroutine to control opening and closing the door
    private IEnumerator FlapDoor()
    {
        // Reverse flap direction if we're already flappin'
        if (isFlapping)
        {
            if (changeSign == -1)
            {
                changeSign = 1;
                currentState = State.Opening;
            }
            else 
            {
                changeSign = -1;
                currentState = State.Closing;
            }
            yield break;
        }

        // Start the animation.
        isFlapping = true;
        
        // Vary this from zero to one, or from one to zero,
        // to interpolate between our quaternions.

        float interpolationParameter;

        // Set lerp parameter to match our state, and the sign
        // of the change to either increase or decrease the
        // lerp parameter during animation.

        if (hinge.localRotation == rotClosed)
        {
            interpolationParameter = 0;
            changeSign = 1;
            currentState = State.Opening;
            // Play opening sound effect because we're opening
            PlaySound(true);
        }
        else
        {
            interpolationParameter = 1;
            changeSign = -1;
            currentState = State.Closing;
        }

        while (isFlapping)
        {
            // Change our "lerp" parameter according to speed and time,
            // and according to whether we are opening or closing.

            interpolationParameter = interpolationParameter + changeSign * Time.deltaTime / flapTime;

            // At or past either end of the lerp parameter's range means
            // we are on our last step.

            if (interpolationParameter >= 1 || interpolationParameter <= 0)
            {
                // Clamp the lerp parameter.

                interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);

                isFlapping = false; // Signal the loop to stop after this.
                
                if (interpolationParameter <= 0)
                {
                    // Play closing sound effect because we just closed
                    PlaySound(false);
                    currentState = State.Closed;
                }
                else
                {
                    currentState = State.Open;
                }
            }

            // Set the X angle to however much rotation is done so far.
            hinge.localRotation = Quaternion.Lerp(rotClosed, rotOpened, interpolationParameter);
            
            // Tell Unity to start us up again at some future time.
            yield return null;
        }
    }
}
