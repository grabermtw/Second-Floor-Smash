using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPunch : StateMachineBehaviour
{
    CharacterController charCtrl;

    public float moveSpeed;
    public bool attack = true;
    public float damageAmount = 6;
    public float launchFactor = 0.2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.gameObject.GetComponent<CharacterController>();
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
            charCtrl.Attack(1.1f, 0.8f, 0, damageAmount, 1.5f, launchFactor);
            charCtrl.Attack(1.1f, 0.8f, 1.5f, damageAmount, 1.5f, launchFactor);
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
