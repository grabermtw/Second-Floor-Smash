using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumControl : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private GameObject currentCharacter;
    private bool rotate = false;
    private AudioSource audioSource;
    private AudioClip[] currentAudios;
    private string prevCharTag;

    // Start is called before the first frame update
    void Awake()
    {
        initialLocalPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(0, 60 * Time.deltaTime, 0);
        }
    }

    public void ToggleRotate()
    {
        rotate = !rotate;
        if (rotate)
        {
            // Make sure that rotating doesn't make them fall off the poduim.
            // This is a lazy solution I know
            Rigidbody charRb = GetComponentInChildren<Rigidbody>();
            if (charRb != null)
            {
                charRb.isKinematic = true;
            }
            
        }
    }

    // Called by Token Control whenever a new character preview is instantiated
    public void AssignCurrentCharacter(GameObject character, AudioClip[] charSelectedAudio, bool forcePlay)
    {
        if (currentCharacter != null)
        {
            prevCharTag = currentCharacter.tag;
        }

        currentCharacter = character;

        // Play audio if the character is different (if their tag name is different).
        // ForcePlay is set to true if you deselect a character then select the same one again.
        // This is needed because that's generally how changing skins works as well.
        if (charSelectedAudio != null && (forcePlay || currentAudios == null || !currentCharacter.CompareTag(prevCharTag)))
        {
            currentAudios = charSelectedAudio;
            audioSource.clip = currentAudios[Random.Range(0, currentAudios.Length)];
            audioSource.Play();
        }
    }

    public void ResetRotation()
    {
        rotate = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Called by FPSToCharSelect, resets the position and clears out any leftover character previews.
    public void ResetPosition()
    {
        transform.localPosition = initialLocalPosition;
        Destroy(currentCharacter);
    }

    public Vector3 GetInitialLocalPosition()
    {
        return initialLocalPosition;
    }
}
