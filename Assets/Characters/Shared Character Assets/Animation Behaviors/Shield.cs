using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : StateMachineBehaviour
{
    CharacterController charCtrl;
    Transform shieldSphere;
    public float damageAmount = 3;
    public float launchFactor = 0.1f;
    int maxShield;
    int shield;
    float shieldShrink; // amount that the shield should shrink by

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.gameObject.GetComponent<CharacterController>();
        // Get our initial value for Shield. This will be the number of frames we will be able to use our
        // shield before it expires
        shield = animator.GetInteger("Shield");

        // Activate the shield gameGbject
        charCtrl.GetShield().SetActive(true);
        // Get its transform
        shieldSphere = charCtrl.GetShield().transform;

        // Calculate how much the shield should shrink by each frame:
        // Get the max possible shield
        maxShield = charCtrl.GetMaxShield();
        shieldShrink = 1f / (float)maxShield;

        // Set starting shield sphere size
        float initSize = 1 - ((maxShield - shield) * shieldShrink);
        shieldSphere.localScale = new Vector3(initSize, initSize, initSize);
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Has the shield expired yet?
        if (shield > 0)
        {
            shield--;
            shieldSphere.localScale = shieldSphere.localScale - new Vector3(shieldShrink, shieldShrink, shieldShrink);

            // Should we dodge?
            int shouldDodge = charCtrl.ShouldWeDodge();
            if(shouldDodge != 0){
                // Yes we should dodge!
                animator.SetInteger("Dodge", shouldDodge);
                Debug.Log("Dodge!");
            }

            // Should we sidestep?
            else if(charCtrl.ShouldWeSidestep()){
                // Yes we should sidestep!
                animator.SetTrigger("Sidestep");
                Debug.Log("Sidestep!");
            }
        }
        else if (shield == 0)
        {
            // Shield has expired!
            animator.SetInteger("Shield", 0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // Did the shield expire? If shield == 0, then it expired and we must punish the player.
        if (shield == 0)
        {
            // Hurt ourselves as punishment
            charCtrl.Strike(damageAmount, 1.4f, 0.01f, charCtrl.GetDirection(), false);
        }
        else
        {
            // Shield didn't run out so we'll recharge it from the amount we have left in the character controller.
            charCtrl.SetCurrentShieldAmt(shield);
        }
        shieldSphere.localScale = new Vector3(1, 1, 1);
        charCtrl.GetShield().SetActive(false);
        animator.ResetTrigger("Sidestep");
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
