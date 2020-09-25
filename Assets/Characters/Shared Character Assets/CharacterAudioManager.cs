using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
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
    public AudioClip upTaunt;
    public AudioClip sideTaunt;
    public AudioClip downTaunt;


    private AudioSource audioSource;


    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayNeutralPunch()
    {
        if (neutralPunch.Length != 0)
        {
            audioSource.clip = neutralPunch[Random.Range(0, neutralPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayUpPunch()
    {
        if (upPunch.Length != 0)
        {
            audioSource.clip = upPunch[Random.Range(0, upPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayDownPunch()
    {
        if (downPunch.Length != 0)
        {
            audioSource.clip = downPunch[Random.Range(0, downPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlaySidePunch()
    {
        if (sidePunch.Length != 0)
        {
            audioSource.clip = sidePunch[Random.Range(0, sidePunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayAirUpPunch()
    {
        if (airUpPunch.Length != 0)
        {
            audioSource.clip = airUpPunch[Random.Range(0, airUpPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayAirDownPunch()
    {
        if (airDownPunch.Length != 0)
        {
            audioSource.clip = airDownPunch[Random.Range(0, airDownPunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayAirSidePunch()
    {
        if (airSidePunch.Length != 0)
        {
            audioSource.clip = airSidePunch[Random.Range(0, airSidePunch.Length)];
            audioSource.Play();
        }
    }

    public void PlayNeutralSpecial()
    {
        if (neutralSpecial.Length != 0)
        {
            audioSource.clip = neutralSpecial[Random.Range(0, neutralSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlayUpSpecial()
    {
        if (upSpecial.Length != 0)
        {
            audioSource.clip = upSpecial[Random.Range(0, upSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlayDownSpecial()
    {
        if (downSpecial.Length != 0)
        {
            audioSource.clip = downSpecial[Random.Range(0, downSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlaySideSpecial()
    {
        if (sideSpecial.Length != 0)
        {
            audioSource.clip = sideSpecial[Random.Range(0, sideSpecial.Length)];
            audioSource.Play();
        }
    }

    public void PlayGrab()
    {
        if (grab.Length != 0)
        {
            audioSource.clip = grab[Random.Range(0, grab.Length)];
            audioSource.Play();
        }
    }

    public void PlayBeingGrabbed()
    {
        if (beingGrabbed.Length != 0)
        {
            audioSource.clip = beingGrabbed[Random.Range(0, beingGrabbed.Length)];
            audioSource.Play();
        }
    }

    public void PlayJump1()
    {
        if (jump1.Length != 0)
        {
            audioSource.clip = jump1[Random.Range(0, jump1.Length)];
            audioSource.Play();
        }
    }

    public void PlayJump2()
    {
        if (jump2.Length != 0)
        {
            audioSource.clip = jump2[Random.Range(0, jump2.Length)];
            audioSource.Play();
        }
    }

    public void PlayTakeDamage()
    {
        if (takeDamage.Length != 0)
        {
            audioSource.clip = takeDamage[Random.Range(0, takeDamage.Length)];
            audioSource.Play();
        }
    }

    public void PlayUpTaunt()
    {
        if (upTaunt != null)
        {
            audioSource.clip = upTaunt;
            audioSource.Play();
        }
    }

    public void PlaySideTaunt()
    {
        if (sideTaunt != null)
        {
            audioSource.clip = sideTaunt;
            audioSource.Play();
        }
    }

    public void PlayDownTaunt()
    {
        if (downTaunt != null)
        {
            audioSource.clip = downTaunt;
            audioSource.Play();
        }
    }
}
