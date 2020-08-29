using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePunch : StateMachineBehaviour
{
    CharacterController charCtrl;
    Transform tf;
    int direction;
    public bool attack = true;
    public float damageAmount = 6;
    public float launchFactor = 0.2f;
    private float moveSpeed = 2f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.gameObject.GetComponent<CharacterController>();
        direction = charCtrl.GetDirection();
        tf = charCtrl.gameObject.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tf.Translate(direction * new Vector3(moveSpeed * Time.deltaTime, 0, 0), Space.World);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(attack)
        {
            charCtrl.Attack(1.16f, 0.8f, 0, damageAmount, 0.75f, launchFactor);
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
