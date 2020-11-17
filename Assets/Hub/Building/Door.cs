using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : HubInteractor
{
    public float maxAngleOpen = 90;
    public float flapTime = 3;
    public AudioClip[] openSounds;
    public AudioClip[] closeSounds;

    Quaternion rotOpened; // Door's rotation when fully opened.
    Quaternion rotClosed; // Door's rotation when full closed.

    bool isFlapping = false; // Animate while true.

    // Set this according to whether we are going from zero
    // to one, or from one to zero.
    float changeSign;

    private Transform hinge;
    private Animator hingeAnim;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        hinge = transform.Find("Hinge"); // let's hope all the doors have hinges...
        hingeAnim = hinge.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rotOpened = Quaternion.Euler(0, maxAngleOpen, 0);
        rotClosed = Quaternion.Euler(0, 0, 0);
    }

    public override void Interact()
    {
        base.playerControl.UnsetInteract();
        StartCoroutine(FlapDoor());
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
        Debug.Log("flapdoor()");
        if (isFlapping)
        {
            changeSign  = (changeSign == -1 ? 1: -1);
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
            // Play opening sound effect because we're opening
            PlaySound(true);
        }
        else
        {
            interpolationParameter = 1;
            changeSign = -1;
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
                }
            }

            // Set the X angle to however much rotation is done so far.
            hinge.localRotation = Quaternion.Lerp(rotClosed, rotOpened, interpolationParameter);
            
            // Tell Unity to start us up again at some future time.
            yield return null;
        }
    }
}
