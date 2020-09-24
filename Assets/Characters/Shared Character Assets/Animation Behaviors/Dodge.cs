using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : StateMachineBehaviour
{
    CharacterController charCtrl;
    Transform tr;
    int direction;
    private DodgeInfo dodgeInfo;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        direction = animator.GetInteger("Dodge"); // -1 for left, 1 for right
        dodgeInfo = animator.gameObject.GetComponent<CommonCombatParams>().GetDodgeInfo();
        charCtrl = dodgeInfo.GetCharCtrl();
        charCtrl.SetDirection(direction);
        tr = animator.gameObject.transform;
        foreach (Collider2D collider in tr.GetComponentsInChildren<Collider2D>(true))
        {
            collider.gameObject.layer = 14; // Dodge layer
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // This is dodging, so we need to move the player in the dodge direction.
        tr.Translate(new Vector3(dodgeInfo.dodgeRate * direction * Time.deltaTime, 0, 0), Space.World);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset colliders to correct non-dodging layer

        int playerNum = charCtrl.GetThisPlayerNumber();
        foreach (Collider2D collider in tr.GetComponentsInChildren<Collider2D>(true))
        {
            collider.gameObject.layer = 8 + playerNum; // Regular player layer
        }
        // Reverse when finished dodging
        charCtrl.SetDirection(-1 * direction);
        animator.SetInteger("Dodge", 0);
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
