using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidestep : StateMachineBehaviour
{

    CharacterController charCtrl;
    Transform tr;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.gameObject.GetComponent<CharacterController>();
        tr = animator.gameObject.transform;

        // Set collider layers to dodge layer
        foreach (Collider2D collider in tr.GetComponentsInChildren<Collider2D>(true))
        {
            collider.gameObject.layer = 14; // Dodge layer
        }
        Debug.Log("Start Sidestep!");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set collider layers back to the usual layer
        int playerNum = charCtrl.GetThisPlayerNumber();
        foreach (Collider2D collider in tr.GetComponentsInChildren<Collider2D>(true))
        {
            collider.gameObject.layer = 8 + playerNum; // Regular player layer
        }
        Debug.Log("Finish Sidestep!");
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
