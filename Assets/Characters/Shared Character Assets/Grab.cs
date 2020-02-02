using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    This script is only to be enabled when the player has successfully grabbed
    another player.
*/

public class Grab : MonoBehaviour
{
    CharacterController charCtrl;
    DamageControl damageControl;
    Animator animator;
    Grabbed victim;
    int direction;
    bool ready = false; // Prevents unwanted execution of the callbacks

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        damageControl = GetComponent<DamageControl>();
        animator = GetComponent<Animator>();

    }

    public void SetReady(bool weGood)
    {
        ready = weGood;
    }

    public void SetVictim(Grabbed vict, int dir)
    {
        if (ready)
        {
            victim = vict;
            direction = dir;
        }
    }

    // Both the regular punch button and the grab button will pummel the victim.
    private void OnPunch()
    {
        if (ready)
        {
            Pummel();
        }
    }

    private void OnGrab()
    {
        if (ready)
        {
            Pummel();
        }
    }

    // Pummels the victim (like a choke basically)
    private void Pummel()
    {
        animator.SetTrigger("Pummel");
        victim.Pummel();
    }


    // Throws the victim and ends the grabbing
    // toss == 0: No throw
    // toss == -1: Throw left
    // toss == 1: Throw right
    // toss == -2: Throw up to the left
    // toss == 2: Throw up to the right
    private void Toss(int toss)
    {
        animator.SetInteger("Toss", Mathf.Abs(toss));
        victim.Release(toss);
        Release();
    }

    // Handle throw directions with left joystick
    private void OnMove(InputValue value)
    {
        if (ready)
        {
            Vector2 dir = value.Get<Vector2>();
            if (Mathf.Abs(dir.x) > 0.6f)
            {
                SideToss(dir.x);
            }
            else if (Mathf.Abs(dir.y) > 0.6f)
            {
                if (direction > 1)
                {
                    Toss(2);
                }
                else
                {
                    Toss(-2);
                }
            }
        }
    }

    // Handle throw directions with right joystick (AKA C-stick)
    private void OnSidePunch(InputValue value)
    {
        if (ready)
        {
            float dir = value.Get<float>();
            SideToss(dir);
        }
    }

    private void SideToss(float dir)
    {
        if (dir > 0.6f)
            {
                // Do we need to turn ourselves (and the victim around?)
                if(charCtrl.GetDirection() == -1){
                    charCtrl.SetDirection(1);
                    Vector3 currentPos = gameObject.transform.position;
                    victim.gameObject.transform.position = new Vector3(currentPos.x + 0.15f, currentPos.y, 0);
                    victim.gameObject.GetComponent<CharacterController>().SetDirection(1);
                }
                Toss(1);
            }
            else if (dir < -0.6f)
            {
                // Do we need to turn ourselves (and the victim around?)
                if(charCtrl.GetDirection() == 1){
                    charCtrl.SetDirection(-1);
                    Vector3 currentPos = gameObject.transform.position;
                    victim.gameObject.transform.position = new Vector3(currentPos.x - 0.15f, currentPos.y, 0);
                    victim.gameObject.GetComponent<CharacterController>().SetDirection(-1);
                }
                Toss(-1);
            }
    }

    private void OnUpPunch()
    {
        if (ready)
        {
            if (direction > 1)
            {
                Toss(2);
            }
            else
            {
                Toss(-2);
            }
        }
    }


    private void OnDownPunch()
    {
        if (ready)
        {
            // For now we're just gonna have side throws and up throws,
            // so down will be another up throw
            OnUpPunch();
        }
    }

    // Done grabbing
    public void Release()
    {
        animator.SetBool("Grabbing", false);
        charCtrl.SetReady(true);
        SetReady(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        animator.ResetTrigger("Pummel");
        animator.SetInteger("Toss", 0);
    }
}