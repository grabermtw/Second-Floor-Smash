using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : StateMachineBehaviour
{
    private CharacterController charCtrl;
    private Transform shieldSphere;
    private ShieldInfo shieldInfo;
    private int maxShield;
    private int shield;
    private float shieldShrink; // amount that the shield should shrink by
    private float shieldRemaining; // amount of remaining shield
    private const float SHIELD_EXPIRE_TIME = 2f; // How much time remaining should there be before shield expires?
                                                // (we don't want the shield to be invisible by the time it expires)

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shieldInfo = animator.gameObject.GetComponent<CommonCombatParams>().GetShieldInfo();
        charCtrl = shieldInfo.GetCharCtrl();
        // Get our initial value for Shield. This will be the number of frames we will be able to use our
        // shield before it expires
        shieldRemaining = (float)animator.GetInteger("Shield");

        // Activate the shield gameGbject
        charCtrl.GetShield().SetActive(true);
        // Get its transform
        shieldSphere = charCtrl.GetShield().transform;

        // Calculate how much the shield should shrink by each frame:
        // Get the max possible shield
        maxShield = charCtrl.GetMaxShield();
        shieldShrink = 1f / (float)maxShield;

        // Set starting shield sphere size
        float initSize = 1 - ((maxShield - shieldRemaining) * shieldShrink);
        shieldSphere.localScale = new Vector3(initSize, initSize, initSize);
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Has the shield expired yet?
        if (shieldRemaining > SHIELD_EXPIRE_TIME) // cut it off early so that it's still visible before the shield expiring animation
        {
            shieldRemaining -= Time.deltaTime;
            shieldSphere.localScale = shieldSphere.localScale - new Vector3(shieldShrink * Time.deltaTime, shieldShrink * Time.deltaTime, shieldShrink * Time.deltaTime);
            Debug.Log("ShieldRemaining: " + shieldRemaining);
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
        else if (shieldRemaining <= SHIELD_EXPIRE_TIME) 
        {
            // Shield has expired!
            animator.SetInteger("Shield", 0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // Did the shield expire? If shield == 0, then it expired and we must punish the player.
        if (shieldRemaining <= SHIELD_EXPIRE_TIME)
        {
            // Hurt ourselves as punishment
            charCtrl.Strike(shieldInfo.damageAmount, shieldInfo.launchAngle, shieldInfo.launchFactor, charCtrl.GetDirection(), false);
            charCtrl.GetCharacterAudioManager().PlayShieldExpire();
        }
        else
        {
            // Shield didn't run out so we'll recharge it from the amount we have left in the character controller.
            charCtrl.SetCurrentShieldAmt((int)shieldRemaining);
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
