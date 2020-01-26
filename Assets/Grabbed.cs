using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    This script is only to be enabled when the player has been grabbed
    by another player.
*/

public class Grabbed : MonoBehaviour
{
    CharacterController charCtrl;
    Animator animator;
    DamageControl damageControl;
    float grabTime;
    Grab attacker;
    bool ready = false; // prevents unwanted execution of the callbacks

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        damageControl = GetComponent<DamageControl>();
    }

    public void SetReady(bool weGood)
    {
        ready = weGood;
        if (ready)
        {
            // Initial grab time is half the player's damage
            grabTime = damageControl.GetDamage() / 2;
        }
    }

    void OnDisable()
    {
        ready = false;
    }

    // Assign our attacker so that we can tell it if we run out of grab time
    public void SetAttacker(Grab assailant)
    {
        attacker = assailant;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            grabTime -= Time.deltaTime;

            // Escape the grab
            if (grabTime <= 0)
            {
                Release();
                attacker.Release();
            }
        }
    }

    // Increase damage by 2.5% every time we're pummeled
    public void Pummel()
    {
        damageControl.UpdateDamage(0.25f);
        animator.SetTrigger("Pummel");
    }

    // Escape from the grab. 
    // toss == 0: No throw
    // toss == -1: Throw left
    // toss == 1: Throw right
    // toss == -2: Throw up to the left
    // toss == 2: Throw up to the right
    public void Release(int toss = 0)
    {
        animator.SetBool("Grabbed", false);
        charCtrl.SetReady(true);
        // Side throw
        if (Mathf.Abs(toss) == 1)
        {
            charCtrl.Strike(3, 0.34f, 0.175f, toss);
        }
        // Up throw
        else if (Mathf.Abs(toss) == 2)
        {
            charCtrl.Strike(3, 1.4f, 0.2f, toss / 2);
        }
        SetReady(false);
    }

    void LateUpdate()
    {
        animator.ResetTrigger("Pummel");
    }

    // The player can speed up their release by button-mashing
    private void ButtonMash()
    {
        if (ready)
        {
            grabTime -= 0.05f;
        }
    }

    // The following are simply all the callbacks from the regular input actions.
    // They'll all just be used for button-mashing.

    private void OnSpecial()
    {
        ButtonMash();
    }

    private void OnPunch()
    {
        ButtonMash();
    }

    private void OnSidePunch()
    {
        ButtonMash();
    }

    private void OnUpPunch()
    {
        ButtonMash();
    }

    private void OnDownPunch()
    {
        ButtonMash();
    }

    private void OnUpTaunt()
    {
        ButtonMash();
    }

    private void OnSideTaunt()
    {
        ButtonMash();
    }

    private void OnDownTaunt()
    {
        ButtonMash();
    }

    private void OnJump()
    {
        ButtonMash();
    }

    private void OnMove()
    {
        ButtonMash();
    }

    private void OnCrouch()
    {
        ButtonMash();
    }

    private void OnShield()
    {
        ButtonMash();
    }

    private void OnGrab()
    {
        ButtonMash();
    }
}
