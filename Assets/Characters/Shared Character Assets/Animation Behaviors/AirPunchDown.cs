using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPunchDown : StateMachineBehaviour
{
    private CharacterController charCtrl;
    private AirPunchDownInfo info;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        info = animator.gameObject.GetComponent<CommonCombatParams>().GetAirPunchDownInfo();
        charCtrl = info.GetCharCtrl();
        charCtrl.Attack(info.firstAttackHeight, info.firstAttackRange,
                        info.firstAttackAngle, info.firstDamageAmount,
                        info.firstLaunchAngle, info.firstLaunchFactor);
        if (info.attackTwice)
        {
            charCtrl.Attack(info.secondAttackHeight, info.secondAttackRange,
            info.secondAttackAngle, info.secondDamageAmount,
            info.secondLaunchAngle, info.secondLaunchFactor);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (info.doubleAttackTwice)
        {
            charCtrl.Attack(info.firstAttackHeight, info.firstAttackRange,
                            info.firstAttackAngle, info.firstDamageAmount,
                            info.firstLaunchAngle, info.firstLaunchFactor);
            if (info.attackTwice)
            {
                charCtrl.Attack(info.secondAttackHeight, info.secondAttackRange,
                info.secondAttackAngle, info.secondDamageAmount,
                info.secondLaunchAngle, info.secondLaunchFactor);
            }
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
