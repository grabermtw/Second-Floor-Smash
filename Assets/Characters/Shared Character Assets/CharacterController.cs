using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CharacterController : MonoBehaviour
{

    Animator animator;
    Vector2 leftJoystick;
    Vector3 movement;
    float moveSpeed = 6f;
    public float damage = 0f;
    int direction;
    Rigidbody2D rb;
    int jumping;
    int currShieldAmt; // Current amount of saved shield we have to use
    bool crouching;
    bool canMove; // Used to determine whether or not the character is "busy" (i.e. in the middle of punching or something)
                  // Assinged in Update()
    bool ready = false; // Set to true at end of Start(), used to prevent unwanted early input from messing things up
    GameObject standCollider;
    GameObject crouchCollider;
    ContactFilter2D attackFilter;
    PlayerNumberManager playerManager;
    TextMeshProUGUI damageDisplay;
    int playerNumber;


    public float jumpPower;
    public float hangOffset; // Distance down from the edge that we must be to realistically hang off the edge.
                             // This really depends on the character's arm/torso length.
    public int maxShield; // Maximum number of frames of shield
    public GameObject shield;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        standCollider = transform.GetChild(0).gameObject;
        crouchCollider = transform.GetChild(1).gameObject;
        canMove = true;
        crouching = false;
        direction = 1;

        // Begin with fully charged shield
        currShieldAmt = maxShield;


        // Get player number and assign it as layer
        playerManager = GameObject.Find("PlayerInputManager").GetComponent<PlayerNumberManager>();
        playerNumber = playerManager.GetPlayerNumber();
        Debug.Log("PLAYER " + playerNumber + " JOINED");
        gameObject.tag = "Player " + playerNumber;
        gameObject.layer = 8 + playerNumber;
        foreach (Collider2D collider in gameObject.transform.GetComponentsInChildren<Collider2D>(true))
        {
            collider.gameObject.layer = 8 + playerNumber;
        }

        // Display the damage reading for this player
        damageDisplay = playerManager.GetDamageText(playerNumber);
        Debug.Log(damageDisplay);
        damageDisplay.text = "0.00%";

        // We only want our attacks to hit the enemy players
        // We must set the appropriate mask based on which player we are.
        attackFilter = new ContactFilter2D();
        if (playerNumber == 1)
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 2", "Player 3", "Player 4"));
        }
        else if (playerNumber == 2)
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 3", "Player 4"));
        }
        else if (playerNumber == 3)
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 2", "Player 4"));
        }
        else
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 2", "Player 3"));
        }

        // Default the Shield animation parameter to -1. This way, later on, if it counts down to 0,
        // then we know that the player used up their shield and must be punished accordingly.
        animator.SetInteger("Shield", -1);

        ready = true;
    }

    // ----------------------- STANDARD ATTACK SECTION ---------------------------------------
    // This section is mostly to trigger the animations for each standard attack.
    // The "Punching", "UpPunch", "SidePunch", "AirPunch" etc scripts attatched to the animation states for
    // each action is where calling the Attack() method to do the damage actually takes place.

    // Handles neutral, side, up, down standard attacks
    private void OnPunch()
    {
        if (ready)
        {
            // Side punch
            if (Mathf.Abs(leftJoystick.x) > 0.4f)
            {
                OnSidePunch();
            }
            // Up punch
            else if (leftJoystick.y > 0.4f)
            {
                OnUpPunch();
            }
            // Down punch
            else if (leftJoystick.y < -0.4f)
            {
                OnDownPunch();
            }
            // Neutral punch
            else if (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("NeutralPunch") ||
                        animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
            {
                Debug.Log("Punch!");
                animator.SetTrigger("Punch");
            }
        }
    }

    private void OnSidePunch(InputValue value = null)
    {
        if (ready)
        {
            float cdirect = 0;
            if (value != null)
            {
                cdirect = value.Get<float>();
            }
            if ((canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("NeutralPunch") ||
                animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) &&
                (cdirect == 0 || Mathf.Abs(cdirect) > 0.4f)) // make sure that the c-stick has been sufficiently pushed
            {
                Debug.Log("Side Punch!");
                // If using C-stick, change direction if necessary
                if (cdirect != 0)
                {
                    SetDirection(cdirect);
                }
                if (jumping == 0)
                {
                    animator.SetTrigger("SidePunch");
                }
                else
                {
                    animator.SetTrigger("Punch");
                }

            }
        }
    }

    private void OnUpPunch(InputValue value = null)
    {
        if (ready && (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) &&
            (value == null || value.Get<float>() > 0.4f))
        {
            animator.SetTrigger("UpPunch");
        }
    }

    private void OnDownPunch(InputValue value = null)
    {
        Debug.Log("DownPunch!");
        if (ready && (canMove || crouching || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) &&
            (value == null || value.Get<float>() > 0.4f))
        {
            animator.SetTrigger("DownPunch");
        }
    }

    // ------------------------ SPECIAL ATTACK SECTION -----------------------------
    // Most of the aspects of the special attacks are actually handled in the character-specific scripts and animations.
    // This section is just to call those functions in the other script.

    // Handles neutral, side, up, down
    private void OnSpecial()
    {
        if (ready)
        {
            // Side punch
            if (Mathf.Abs(leftJoystick.x) > 0.4f)
            {
                gameObject.SendMessage("OnSideSpecial", leftJoystick.x > 0 ? 1 : -1, SendMessageOptions.DontRequireReceiver);
                Debug.Log("Side Special");
            }
            // Up punch
            else if (leftJoystick.y > 0.4f)
            {
                gameObject.SendMessage("OnUpSpecial", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Up Special");
            }
            // Down punch
            else if (leftJoystick.y < -0.4f)
            {
                gameObject.SendMessage("OnDownSpecial", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Down Special");
            }
            // Neutral punch
            else if (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("NeutralPunch") ||
                        animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
            {
                gameObject.SendMessage("OnNeutralSpecial", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Neutral Special");
            }
        }
    }

    // ----------------------- JUMPING SECTION ---------------------------

    private void OnJump()
    {
        if(ready){
            if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Hanging")){
                Debug.Log("Jump from haning!");
                jumping = 1;
                animator.SetInteger("Jumping", jumping); // start jump animation
                rb.isKinematic = false;
                rb.AddForce(transform.up * jumpPower);
            }
        if (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) // Shouldn't be able to jump while punching
        {
            Debug.Log("Enter jump");
            // First Jump
            if (rb.IsTouchingLayers(LayerMask.GetMask("Platform")))
            {
                StopCrouch();
                Debug.Log("Single Jump!");
                jumping = 1;
                animator.SetInteger("Jumping", jumping); // start jump animation
                rb.AddForce(transform.up * jumpPower);
            }
            // Double Jump
            else if (jumping == 1)
            {
                Debug.Log("Double Jump!");
                jumping = 2;
                animator.SetInteger("Jumping", jumping);
                // reset the verticle velocity so that our jumping force isn't hindered by a downward velocity.
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(transform.up * jumpPower);
            }
        }
        }
    }

    // Use collision to let us know when we've finished jumping
    void OnCollisionEnter2D(Collision2D col)
    {
        // Make sure that the layer we are colliding with is the "Platform" layer.
        // The "Wall" layer exists to be used for any parts of platforms that should not grant
        // the player another jump, such as the sides and the bottom of the platform.
        if (col.gameObject.layer == 8)
        {
            Debug.Log("land!");
            jumping = 0;
            animator.SetInteger("Jumping", jumping);
        }
    }

    // Use this to treat walking off the edge of a platform or being knocked into the sky as a single jump
    void OnCollisionExit2D(Collision2D col)
    {
        // Whenever we stop touching a platform, we are allowed to do 1 jump.
        // This allows us to perform the second jump if we walk off a platform or if someone
        // hits us into the air.
        if (col.gameObject.layer == 8)
        {
            jumping = 1;
        }
    }

    // -------- TAUNT SECTION -----------------
    // To add a taunt, just add it to the animation controller and use the appropriate trigger.

    private void OnUpTaunt()
    {
        if (ready)
        {
            animator.SetTrigger("UpTaunt");
            Debug.Log("UpTaunt");
        }
    }

    private void OnSideTaunt()
    {
        if (ready)
        {
            animator.SetTrigger("SideTaunt");
            Debug.Log("SideTaunt");
        }
    }

    private void OnDownTaunt()
    {
        if (ready)
        {
            animator.SetTrigger("DownTaunt");
            Debug.Log("DownTaunt");
        }
    }

    // ---------------------- Crouch Section ----------------------

    private void OnCrouch(InputValue value)
    {
        if (ready)
        {
            Debug.Log("crouching!");
            if (value.Get<float>() > 0.6f)
            {
                crouching = true;
                standCollider.SetActive(false);
                crouchCollider.SetActive(true);
                animator.SetBool("Crouch", crouching);
            }
            else
            {
                StopCrouch();
            }
        }
    }

    private void StopCrouch()
    {
        crouching = false;
        animator.SetBool("Crouch", crouching);
        crouchCollider.SetActive(false);
        standCollider.SetActive(true);
    }

    // Are we currently crouching? (Used by the downpunch script)
    public bool IsCrouching()
    {
        return crouching;
    }

    // ---------------- SHIELD/DODGE/SIDESTEP SECTION --------------------

    // Below we handle Shield, and provide helper methods for dodging and sidestepping.
    // The main portion of handling dodging and sidestepping will take place in the Shield animation behavior.

    // The binding for the Shield action is "Press and Release", meaning that this will be called once
    // when the button is pressed and once when it is released.
    private void OnShield()
    {
        if (ready)
        {
            // Is the shield currently in use? (Is this the press or the release of the button?)
            if (animator.GetInteger("Shield") == -1 && (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")))
            {
                Debug.Log("Shield!");
                animator.SetInteger("Shield", currShieldAmt); // currShieldAmt frames of shield!
                                                              // The Shield script attatched to the animation state will count this down.

                /* Set the character's children with the colliders to the Shield Layer (layer 13).
                   The Shield layer is not in the layermask, so it cannot be attacked with Attack(),
                   however, things can still collide with it.
                */
                foreach (Collider2D collider in gameObject.transform.GetComponentsInChildren<Collider2D>(true))
                {
                    collider.gameObject.layer = 13; // Shield layer
                }
            }
            else // End the shield usage
            {
                Debug.Log("Shield Deactivated!"); // We, the player, have deactivated the shield
                animator.SetInteger("Shield", -1); // Reset the anim param to -1, the indicator that we're not using shield

                // Reset the colliders' layer back to "Player X" so that we can take damage again
                foreach (Collider2D collider in gameObject.transform.GetComponentsInChildren<Collider2D>(true))
                {
                    collider.gameObject.layer = 8 + playerNumber;
                }
            }
        }
    }

    // Sets the currShieldAmt variable, called by the shield animation behavior when
    // the player ends the shield before it runs out.
    public void SetCurrentShieldAmt(int amount)
    {
        currShieldAmt = amount;
    }

    // Returns the shield gameobject, used in the Shield animation behavior
    public GameObject GetShield()
    {
        return shield;
    }

    // Gets the maximum possible shield, used for scaling the actual shield sphere in the shield behavior script
    public int GetMaxShield()
    {
        return maxShield;
    }

    // These two methods return the gameObjects for the standing and the crouching colliders,
    // helpful for if you faint after your shield expires.

    public GameObject GetStandingCollider()
    {
        return standCollider;
    }

    public GameObject GetCrouchingCollider()
    {
        return crouchCollider;
    }

    // This function is called every frame by the Shield animation behavior while shielding.
    // If we should dodge, it returns the dodge direction (-1 or 1), otherwise, it returns 0.
    public int ShouldWeDodge(){
        if(leftJoystick.x > 0.4f){
            // Yes, we should dodge to the right.
            return 1;
        }
        else if(leftJoystick.x < -0.4f){
            // Yes, we should dodge to the left.
            return -1;
        }
        else{
            // No, we should not dodge.
            return 0;
        }
    }

    // This function is much like ShouldWeDodge(), only it returns a bool cause direction is irrelevant for sidestep
    public bool ShouldWeSidestep(){
        
        return leftJoystick.y < -0.4f;
    }

    // For dodging/sidestepping, most of the logic takes place within the animation behavior script, so we'll
    // need to be able to give it the player number so that we can adjust the collision layer accordingly
    public int GetThisPlayerNumber()
    {
        return playerNumber;
    }


    // --------------- LATERAL MOVEMENT SECTION ---------------------

    // OnMove is important not only for moving, as it updates the leftJoystick vector2,
    // which is used in determining directions of standard and special attacks
    private void OnMove(InputValue value)
    {
        if (ready)
        {
            leftJoystick = value.Get<Vector2>();
        }
    }

    void Update()
    {

        // Check to see if the current state is tagged as "Moveable", such as in the "Movement" and "Jumping" states
        canMove = animator.GetCurrentAnimatorStateInfo(0).IsTag("Moveable");

        // If we don't have max shield at the moment and we aren't using our shield, recharge shield!
        if (maxShield > currShieldAmt && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Shield"))
        {
            currShieldAmt++;
        }

        if (!crouching && canMove && !rb.isKinematic)
        {
            StopCrouch();
            Move();
        }
    }

    void Move()
    {
        // Move right
        if (leftJoystick.x > 0)
        {
            direction = 1;
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.velocity = (new Vector2(rb.velocity.x / 1.02f, rb.velocity.y));
        }
        // Move left
        else if (leftJoystick.x < 0)
        {
            direction = -1;
            transform.eulerAngles = new Vector3(0, 180, 0);
            rb.velocity = (new Vector2(rb.velocity.x / 1.02f, rb.velocity.y));
        }
        movement = new Vector3(leftJoystick.x, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Adjust our idle/walk/run animation blend tree
        animator.SetFloat("Run", Mathf.Abs(movement.x) * 100f);
    }

    // Helper methods in case we need it later
    public void SetCanMove(bool can)
    {
        canMove = can;
    }

    public int GetDirection()
    {
        return direction;
    }

    public void SetDirection(float dir)
    {
        if (dir < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            direction = -1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            direction = 1;
        }
    }

    // --------------------------- EDGE GRAB/HANG SECTION ----------------------------

    // Called by the EdgeHangTrigger script whenever we enter the edge hang trigger
    // Arguments indicate if it is a left edge (-1) or a right edge (1), as well as the edge's position
    public void Hang(int edgeDir, Vector3 edgePos)
    {
        // We want to face right if its a left edge and vice versa
        SetDirection(edgeDir * -1);

        // Position ourselves to be hanging on the edge
        transform.position = new Vector3(edgePos.x + (0.05f * edgeDir), edgePos.y - hangOffset, 0);
        animator.SetTrigger("Hang");
        Debug.Log("Hang!");
        jumping = 0;
        animator.SetInteger("Jumping", jumping);
    }

    


    // --------------------------- DEALING DAMAGE SECTION -----------------------

    public void Attack(float startHeight, float range, float angle, float addDamage, float launchAngle,
    float launchFactor, bool useHitAnim = true)
    {
        Vector2 origin = new Vector2(rb.position.x, rb.position.y + startHeight);
        Vector2 castDirection = new Vector2(Mathf.Cos(angle) * direction, Mathf.Sin(angle));
        RaycastHit2D[] results = new RaycastHit2D[5];

        int numHits = Physics2D.Raycast(origin, castDirection, attackFilter, results, range);

        // TODO add striking to all struck things
        for (int i = 0; i < numHits; i++)
        {
            results[i].rigidbody.gameObject.GetComponent<CharacterController>().Strike(addDamage, launchAngle, launchFactor, direction, useHitAnim);
        }
    }


    // -------------------- TAKING DAMAGE SECTION ------------------------

    // angle is in radians
    // direction is -1 for left, 1 for right
    public void Strike(float addDamage, float angle, float launchFactor, int launchDirection, bool useHitAnim = true)
    {
        damage += addDamage;
        float launchEnergy = damage * launchFactor;

        if (useHitAnim) // by default, we use this, but in certain cases (such as shield expiring) we don't want this
        {
            // handle direction of hit animation
            // Both Player face right
            // launchDirection = 1
            // direction = 1
            // HitDirection = 1
            if (launchDirection != direction)
            {
                animator.SetFloat("HitDirection", -1);
            }
            else
            {
                animator.SetFloat("HitDirection", 1);
            }
            animator.SetTrigger("Hit");
        }
        // Increase the damage score on the read-out
        UpdateDamage();

        // Add our force to launch us
        rb.AddForce(new Vector2(launchEnergy * Mathf.Cos(angle) * launchDirection, launchEnergy * Mathf.Sin(angle)), ForceMode2D.Impulse);
    }

    // Helper methods for other things
    public float GetDamage()
    {
        return damage;
    }

    private void UpdateDamage()
    {
        damageDisplay.text = damage.ToString("F") + "%";
    }




    // Use LateUpdate to reset the triggers so that they can be evaluated within the animation controller
    // and then reset all within the same frame.
    void LateUpdate()
    {
        // Exclude "Sidestep" from this because it needs to be reset in Shield's OnStateExit for it to work.
        animator.ResetTrigger("Punch");
        animator.ResetTrigger("SidePunch");
        animator.ResetTrigger("UpPunch");
        animator.ResetTrigger("DownPunch");
        animator.ResetTrigger("Hang");
    }

}