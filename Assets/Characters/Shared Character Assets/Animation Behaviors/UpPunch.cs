using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPunch : StateMachineBehaviour
{
    private CharacterController charCtrl;
    private UpPunchInfo info;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info = animator.gameObject.GetComponent<CommonCombatParams>().GetUpPunchInfo();
        charCtrl = info.GetCharCtrl();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //        
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl.Attack(info.attackHeight, info.attackRange, info.firstAttackAngle, info.damageAmount, info.launchAngle, info.launchFactor);
        if (info.attackTwice)
        {
            charCtrl.Attack(info.attackHeight, info.attackRange, info.secondAttackAngle, info.damageAmount, info.launchAngle, info.launchFactor);
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
