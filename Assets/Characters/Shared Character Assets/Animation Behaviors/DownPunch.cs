using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPunch : StateMachineBehaviour
{

    CharacterController charCtrl;
    GameObject standCollide;
    GameObject crouchCollide;

    public float moveSpeed;
    public bool attack = true;
    public float damageAmount = 6;
    public float launchFactor = 0.2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.gameObject.GetComponent<CharacterController>();
        if (attack)
        {
            charCtrl.Attack(0.1f, 1.5f, 0, damageAmount, 1.5f, launchFactor);
        }
        standCollide = charCtrl.GetStandingCollider();
        crouchCollide = charCtrl.GetCrouchingCollider();
        standCollide.SetActive(false);
        crouchCollide.SetActive(true);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attack)
        {
            charCtrl.Attack(0.1f, 0.8f, 3.14f, damageAmount, 1.74f, launchFactor);
        }
        if (!charCtrl.IsCrouching())
        {
            crouchCollide.SetActive(false);
            standCollide.SetActive(true);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
