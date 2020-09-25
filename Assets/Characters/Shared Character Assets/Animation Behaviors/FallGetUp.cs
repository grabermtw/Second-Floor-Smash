using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGetUp : StateMachineBehaviour
{
    CharacterController charCtrl;
    GameObject standCollide;
    GameObject crouchCollide;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.gameObject.GetComponent<CharacterController>();
        standCollide = charCtrl.GetStandingCollider();
        crouchCollide = charCtrl.GetCrouchingCollider();

        standCollide.SetActive(false);
        crouchCollide.SetActive(true);
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("GetUp") && stateInfo.normalizedTime > 0.6f && crouchCollide.activeSelf){
            Debug.Log("Reactivating standing collider");
            standCollide.SetActive(true);
            crouchCollide.SetActive(false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
