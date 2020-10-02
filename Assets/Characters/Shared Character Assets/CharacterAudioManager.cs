using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CharacterAudioManager : MonoBehaviour
{
    public AudioClip[] characterSelected; // used for character select screen
    public AudioClip[] neutralPunch;
    public AudioClip[] upPunch;
    public AudioClip[] downPunch;
    public AudioClip[] sidePunch;
    public AudioClip[] airSidePunch;
    public AudioClip[] airUpPunch;
    public AudioClip[] airDownPunch;
    public AudioClip[] neutralSpecial;
    public AudioClip[] upSpecial;
    public AudioClip[] downSpecial;
    public AudioClip[] sideSpecial;
    public AudioClip[] grab;
    public AudioClip[] beingGrabbed;
    public AudioClip[] jump1;
    public AudioClip[] jump2;
    public AudioClip[] takeDamage;
    public AudioClip[] shieldExpire;
    public AudioClip[] upTaunt;
    public AudioClip[] sideTaunt;
    public AudioClip[] downTaunt;


    private AudioSource audioSource;


    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Get the correct audioMixer to assign to our audio source.
        // Each audio mixer will be the scene name (note SCENE name not STAGE name) with "Mixer" appended to the end of it.
        try
        {
            AudioMixer mixer = Resources.Load("StageAudioMixers/" + SceneManager.GetActiveScene().name + "Mixer") as AudioMixer;
            // Find the "CharSoundFX" group within the audio mixer and assign it to our audio source.
            audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("CharSoundFX")[0];
        } catch {
            Debug.LogWarning("This scene, " + SceneManager.GetActiveScene().name + ", lacks a properly-titled AudioMixer." +
                            " In the folder for this stage, there should be an AudioMixer called \"" + SceneManager.GetActiveScene().name + "Mixer" +
                            "\", and it should have an audio mixer group called \"CharSoundFX\".");
        }
    }

    // Returns a random characterSelected audioClip for the player. This is only used
    // for getting the audio to play in the character select screen.
    public AudioClip[] GetCharacterSelectedAudioClip()
    {
        if (characterSelected.Length != 0)
        {
            return characterSelected;
        }
        else 
        {
            return null;
        }
    }

    public void PlayNeutralPunch()
    {
        if (neutralPunch.Length != 0 && !IsPlayingInArr(neutralPunch))
        {
            audioSource.clip = neutralPunch[Random.Range(0, neutralPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayUpPunch()
    {
        if (upPunch.Length != 0 && !IsPlayingInArr(upPunch))
        {
            audioSource.clip = upPunch[Random.Range(0, upPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayDownPunch()
    {
        if (downPunch.Length != 0 && !IsPlayingInArr(downPunch))
        {
            audioSource.clip = downPunch[Random.Range(0, downPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlaySidePunch()
    {
        if (sidePunch.Length != 0 && !IsPlayingInArr(sidePunch))
        {
            audioSource.clip = sidePunch[Random.Range(0, sidePunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayAirUpPunch()
    {
        if (airUpPunch.Length != 0 && !IsPlayingInArr(airUpPunch))
        {
            audioSource.clip = airUpPunch[Random.Range(0, airUpPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayAirDownPunch()
    {
        if (airDownPunch.Length != 0 && !IsPlayingInArr(airDownPunch))
        {
            audioSource.clip = airDownPunch[Random.Range(0, airDownPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayAirSidePunch()
    {
        if (airSidePunch.Length != 0 && !IsPlayingInArr(airSidePunch))
        {
            audioSource.clip = airSidePunch[Random.Range(0, airSidePunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayNeutralSpecial()
    {
        if (neutralSpecial.Length != 0 && !IsPlayingInArr(neutralSpecial))
        {
            audioSource.clip = neutralSpecial[Random.Range(0, neutralSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlayUpSpecial()
    {
        if (upSpecial.Length != 0 && !IsPlayingInArr(upSpecial))
        {
            audioSource.clip = upSpecial[Random.Range(0, upSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlayDownSpecial()
    {
        if (downSpecial.Length != 0 && !IsPlayingInArr(downSpecial))
        {
            audioSource.clip = downSpecial[Random.Range(0, downSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlaySideSpecial()
    {
        if (sideSpecial.Length != 0 && !IsPlayingInArr(sideSpecial))
        {
            audioSource.clip = sideSpecial[Random.Range(0, sideSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlayGrab()
    {
        if (grab.Length != 0 && !IsPlayingInArr(grab))
        {
            audioSource.clip = grab[Random.Range(0, grab.Length)];
            audioSource.Play();
        }
    }

    public void PlayBeingGrabbed()
    {
        if (beingGrabbed.Length != 0 && !IsPlayingInArr(beingGrabbed))
        {
            audioSource.clip = beingGrabbed[Random.Range(0, beingGrabbed.Length)];
            audioSource.Play();
        }
    }

    public void PlayJump1()
    {
        if (jump1.Length != 0 && !IsPlayingInArr(jump1))
        {
            audioSource.clip = jump1[Random.Range(0, jump1.Length)];
            audioSource.Play();
        }
    }

    public void PlayJump2()
    {
        if (jump2.Length != 0 && !IsPlayingInArr(jump2))
        {
            audioSource.clip = jump2[Random.Range(0, jump2.Length)];
            audioSource.Play();
        }
    }

    public void PlayTakeDamage()
    {
        if (takeDamage.Length != 0 && !IsPlayingInArr(takeDamage))
        {
            AudioClip clip = takeDamage[Random.Range(0, takeDamage.Length)];
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void PlayShieldExpire()
    {
        if (shieldExpire.Length != 0 && !IsPlayingInArr(shieldExpire))
        {
            AudioClip clip = shieldExpire[Random.Range(0, shieldExpire.Length)];
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }


    // Something weird happened with the taunts so a lot of people have empty spots
    // in their taunt sound arrays, so we're gonna do an extra null check here.
    public void PlayUpTaunt()
    {
        if (upTaunt.Length != 0 && !IsPlayingInArr(upTaunt))
        {
            AudioClip clip = upTaunt[Random.Range(0, upTaunt.Length)];
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void PlaySideTaunt()
    {
        if (sideTaunt.Length != 0 && !IsPlayingInArr(sideTaunt))
        {
            AudioClip clip = sideTaunt[Random.Range(0, sideTaunt.Length)];
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void PlayDownTaunt()
    {
        if (downTaunt.Length != 0 && !IsPlayingInArr(downTaunt))
        {
            AudioClip clip = downTaunt[Random.Range(0, downTaunt.Length)];
            if(clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }



    // ---------- HELPERS -----------

    // returns true if the audio clip currently playing in the specified array.
    // Used in all the Play_____() methods so that we don't get people spamming
    // the same audio clip that sounds awful.
    private bool IsPlayingInArr(AudioClip[] clipArr)
    {
        if (audioSource.isPlaying)
        {
            return clipArr.Contains(audioSource.clip);
        }
        return false;
    }

}
