using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement; // remove this if no longer needed

public class CharacterController : MonoBehaviour
{
    private int maxShield = 7; // Maximum number of seconds of shield
    private Animator animator;
    private Vector2 leftJoystick;
    private Vector3 movement;
    private float moveSpeed = 6f;
    private int direction;
    private Rigidbody2D rb;
    private CharacterAudioManager charAudio;
    private int jumping;
    private int currShieldAmt; // Current amount of saved shield we have to use
    private bool crouching;
    private bool canMove; // Used to determine whether or not the character is "busy" (i.e. in the middle of punching or something)
                  // Assinged in Update()
    private bool ready = false; // Set to true at end of Start(), used to prevent unwanted early input from messing things up
                        // Also set to true whenever the script is enabled and set to false whenever it is disabled.
                        //bool started = false; // Set to true once and only once, at the end of start.
                        // Prevents OnEnable from prematurely setting ready to true.

    private GameObject standCollider;
    private GameObject crouchCollider;
    private ContactFilter2D attackFilter;
    private ContactFilter2D grabFilter;
    private DamageControl damageControl;
    private int playerNumber;
    private Dictionary<string, int> characterIndices; // Used for determining the int that indicates what special
                                                      // moves the characters have

    public float jumpPower;
    public float hangOffset; // Distance down from the edge that we must be to realistically hang off the edge.
                             // This really depends on the character's arm/torso length.
    
    public float grabHeight; // Height on the player that grabbing occurs at
    public float grabRange; // How far is the player's reach?
    public GameObject shield;
    public GameObject nonGameCharacter; // This is a reference to the "Non-Game" version of the character,
                                        // for use in opening animation and basically any 3D environment


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageControl = GetComponent<DamageControl>();
        charAudio = GetComponent<CharacterAudioManager>();

        // Initialize the dictionary with all the character tags
        characterIndices = new Dictionary<string, int>()
        {
            {"John Ball", 1},
            {"Johnny Three Sides", 1},
            {"Dr. Desmesmert", 1},
            {"Omer Bowman", 2},
            {"Jordan Woo", 3},
            {"The Ghost of Prince Frederick", 3},
            {"DefaultCharacter", 4},
            {"AJ Shannon", 5},
            {"Anders Julin", 6},
            {"Calvin Crunkleton", 7},
            {"Graber", 8},
            {"James Hall", 9},
            {"Jesse Parreira", 10},
            {"Vorsteg", 11},
            {"Tim Henderson", 12},
            {"Agent Timothy Henderson", 12},
            {"Tim from Tim's Tastings", 12},
            {"Tim O. Thee", 12},
            {"Patrick Vorsteg", 13},
            {"Nate Rogers", 14},
            {"Beauregard Bowman", 14},
            {"Quinn Morris", 15},
            {"Emma Mirizio", 16},
            {"Judy Tram", 17},
            {"Christina Huang", 18},
            {"Jasmine Lim", 19},
            {"Tom Zong", 20},
            {"Tom from Tom's Tastings", 20},
            {"Emily Whittaker", 21},
            {"Dr. Whittaker", 21},
            {"Fred Delawie", 22},
            {"Felix Adams", 23},
            {"Joy London", 24},
            {"June Xu", 25},
            {"Flo Ning", 26},
            {"Bebo Harraz", 27},
            {"Melissa Baum", 28},
            {"Testudo", 29},
            {"VorstegBot", 30},
            {"Robois", 30},
            {"GraberBot", 30},
            {"OmerBot", 30},
            {"Robo Farmer", 30},
            {"Standard Bad Guy", 30},
            {"Meghan Graber", 31},
            {"Brian DeLorenzo", 32},
            {"Sam Polhemus", 33},
            {"Ben Eassa", 34},
            {"Bella Lay", 35},
            {"Lore Dixon", 36},
            {"Danielle Kennedy", 37},
            {"Dave Hoag", 38},
            {"Milan Gupta", 39},
            {"Molly Oyer", 40},
            {"Kate Benware", 41},
            {"Chico Liu", 52},
            {"Cormac Bowman", 53},
            {"Maria Oliva", 54},
            {"Amanda O'Shaughnessy", 55},
            {"Elizabeth Kilpatrick", 56},
            {"D.O.U.G.", 57},
            {"Jim Boeheim", 58},
            {"Jimbo", 58}
        };
    }

    void Start()
    {
        standCollider = transform.GetChild(0).gameObject;
        crouchCollider = transform.GetChild(1).gameObject;
        canMove = true;
        crouching = false;
        direction = 1;

        // Begin with fully charged shield
        currShieldAmt = maxShield;


        // Get player number from the layer (it would've been given immediately after we were instantiated)
        playerNumber = gameObject.layer - 8;
        Debug.Log("PLAYER " + playerNumber + " JOINED");
        //gameObject.tag = "Player " + playerNumber;
        foreach (Collider2D collider in gameObject.transform.GetComponentsInChildren<Collider2D>(true))
        {
            collider.gameObject.layer = 8 + playerNumber;
        }

        // Inform DamageControl of what our number is
        damageControl.AssignPlayerNumber(playerNumber);

        // We only want our attacks to hit the enemy players
        // We must set the appropriate mask based on which player we are.
        attackFilter = new ContactFilter2D();
        // Same goes for grabbing, except that grabbing should work against shielded players
        grabFilter = new ContactFilter2D();
        // We include the "Wall" layer in the masks because we will use that layer to shorten an attack's range if that's in
        // the way of an opponent
        if (playerNumber == 1)
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 2", "Player 3", "Player 4", "Wall"));
            grabFilter.SetLayerMask(LayerMask.GetMask("Player 2", "Player 3", "Player 4", "Shield", "Wall"));
        }
        else if (playerNumber == 2)
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 3", "Player 4", "Wall"));
            grabFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 3", "Player 4", "Shield", "Wall"));
        }
        else if (playerNumber == 3)
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 2", "Player 4", "Wall"));
            grabFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 2", "Player 4", "Shield", "Wall"));
        }
        else
        {
            attackFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 2", "Player 3", "Wall"));
            grabFilter.SetLayerMask(LayerMask.GetMask("Player 1", "Player 2", "Player 3", "Shield", "Wall"));
        }

        // Default the Shield animation parameter to -1. This way, later on, if it counts down to 0,
        // then we know that the player used up their shield and must be punished accordingly.
        animator.SetInteger("Shield", -1);

        // Inform the animator of who we are
        animator.SetInteger("CharacterIndex", GetCharacterAnimIndex());

        ready = true;
    }

    public void SetReady(bool weGood)
    {
        ready = weGood;
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
                charAudio.PlayNeutralPunch();
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
                    charAudio.PlaySidePunch();
                }
                else
                {
                    animator.SetTrigger("Punch");
                    charAudio.PlayAirSidePunch();
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
            charAudio.PlayUpPunch();
        }
    }

    private void OnDownPunch(InputValue value = null)
    {
        Debug.Log("DownPunch!");
        if (ready && (canMove || crouching || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) &&
            (value == null || value.Get<float>() > 0.4f))
        {
            animator.SetTrigger("DownPunch");
            charAudio.PlayDownPunch();
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
            // Side special
            if (Mathf.Abs(leftJoystick.x) > 0.4f && (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")))
            {
                gameObject.SendMessage("OnSideSpecial", leftJoystick.x > 0 ? 1 : -1, SendMessageOptions.DontRequireReceiver);
                animator.SetTrigger("SideSpecial");
                charAudio.PlaySideSpecial();
                Debug.Log("Side Special");
            }
            // Up special
            else if (leftJoystick.y > 0.4f && (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")))
            {
                gameObject.SendMessage("OnUpSpecial", SendMessageOptions.DontRequireReceiver);
                animator.SetTrigger("UpSpecial");
                charAudio.PlayUpSpecial();
                Debug.Log("Up Special");
            }
            // Down special
            else if (leftJoystick.y < -0.4f && (canMove || crouching || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")))
            {
                gameObject.SendMessage("OnDownSpecial", SendMessageOptions.DontRequireReceiver);
                charAudio.PlayDownSpecial();
                animator.SetTrigger("DownSpecial");
            }
            // Neutral special
            else if (canMove || animator.GetCurrentAnimatorStateInfo(0).IsTag("NeutralPunch") ||
                        animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
            {
                gameObject.SendMessage("OnNeutralSpecial", SendMessageOptions.DontRequireReceiver);
                animator.SetTrigger("NeutralSpecial");
                charAudio.PlayNeutralSpecial();
                Debug.Log("Neutral Special");
            }
        }
    }

    // ----------------------- GRABBING SECTION --------------------------
    // There are separate scripts devoted to grabbing, but this is what gets that going.
    // The "Grab" script is for the assailant, and the "Grabbed" script is for the victim.

    // This method called when we're gonna try to grab
    private void OnGrab()
    {
        if (ready && canMove)
        {
            Vector2 origin = new Vector2(rb.position.x, rb.position.y + grabHeight);
            Vector2 castDirection = new Vector2(Mathf.Cos(0) * direction, Mathf.Sin(0));
            RaycastHit2D[] results = new RaycastHit2D[5];

            animator.SetTrigger("Grab"); // begin the grabbing animation

            int numHits = Physics2D.Raycast(origin, castDirection, grabFilter, results, grabRange);

            // If someone was in range, grab them!
            if (numHits > 0)
            {
                // Grab script
                Grab ourGrab = GetComponent<Grab>();
                results[0].rigidbody.gameObject.GetComponent<CharacterController>().Grabbed(transform.position, direction, ourGrab);
                animator.SetBool("Grabbing", true);

                // Switch to the "Grab" script and disable the character controller
                ourGrab.SetReady(true);
                // Give the Grab script our victim and direction
                ourGrab.SetVictim(results[0].rigidbody.gameObject.GetComponent<Grabbed>(), direction);
                Debug.Log("We've grabbed someone, deactivating character controller!");
                ready = false;
                charAudio.PlayGrab();
            }
            else // Didn't grab anyone
            {
                animator.SetBool("Grabbing", false);
            }
        }
    }

    // This method called when we have been grabbed
    public void Grabbed(Vector3 assailantPos, int dir, Grab attacker)
    {
        // Make sure we're facing the same way as our attacker (they'll grab us from behind)
        SetDirection(dir);
        // Put ourselves at a good stranglin' distance
        transform.position = new Vector3(assailantPos.x + dir * 0.15f, assailantPos.y, 0);
        // Let the animator know we're being grabbed
        animator.SetBool("Grabbed", true);
        animator.SetTrigger("BeginGrabbed");

        // Switch to the "Grabbed" script and disable the character controller
        Grabbed ourGrabbed = GetComponent<Grabbed>();
        ourGrabbed.SetReady(true);
        ourGrabbed.SetAttacker(attacker);
        Debug.Log("We've been grabbed, deactivating character controller");
        ready = false;
        charAudio.PlayBeingGrabbed();
    }

    // ----------------------- JUMPING SECTION ---------------------------

    private void OnJump()
    {
        if (ready)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hanging"))
            {
                Debug.Log("Jump from haning!");
                jumping = 1;
                animator.SetInteger("Jumping", jumping); // start jump animation
                rb.isKinematic = false;
                rb.AddForce(transform.up * jumpPower);
                charAudio.PlayJump1();
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
                    charAudio.PlayJump1();
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
                    charAudio.PlayJump2();
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
            charAudio.PlayUpTaunt();
            Debug.Log("UpTaunt");
        }
    }

    private void OnSideTaunt()
    {
        if (ready)
        {
            animator.SetTrigger("SideTaunt");
            charAudio.PlaySideTaunt();
            Debug.Log("SideTaunt");
        }
    }

    private void OnDownTaunt()
    {
        if (ready)
        {
            animator.SetTrigger("DownTaunt");
            charAudio.PlayDownTaunt();
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
    public int ShouldWeDodge()
    {
        if (leftJoystick.x > 0.4f)
        {
            // Yes, we should dodge to the right.
            return 1;
        }
        else if (leftJoystick.x < -0.4f)
        {
            // Yes, we should dodge to the left.
            return -1;
        }
        else
        {
            // No, we should not dodge.
            return 0;
        }
    }

    // This function is much like ShouldWeDodge(), only it returns a bool cause direction is irrelevant for sidestep
    public bool ShouldWeSidestep()
    {

        return leftJoystick.y < -0.4f;
    }

    // For dodging/sidestepping, most of the logic takes place within the animation behavior script, so we'll
    // need to be able to give it the player number so that we can adjust the collision layer accordingly.
    // Also used in other stage-specific scripts.
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
        movement = new Vector3(leftJoystick.x, 0, 0) * moveSpeed;
        transform.Translate(movement  * Time.deltaTime, Space.World);

        // Adjust our idle/walk/run animation blend tree
        animator.SetFloat("Run", Mathf.Abs(movement.x) * 2);
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
            try
            {
                results[i].rigidbody.gameObject.GetComponent<CharacterController>().Strike(addDamage, launchAngle, launchFactor, direction, useHitAnim);
            }
            catch // Error when we hit a wall, this will stop our attack, which is what we want
            {
                break;
            }
        }
    }


    // -------------------- TAKING DAMAGE SECTION ------------------------

    // angle is in radians
    // direction is -1 for left, 1 for right
    public void Strike(float addDamage, float angle, float launchFactor, int launchDirection, bool useHitAnim = true)
    {
        damageControl.UpdateDamage(addDamage);
        float launchEnergy = damageControl.GetDamage() * launchFactor;

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

        // Add our force to launch us
        rb.AddForce(new Vector2(launchEnergy * Mathf.Cos(angle) * launchDirection, launchEnergy * Mathf.Sin(angle)), ForceMode2D.Impulse);
        charAudio.PlayTakeDamage();
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
        animator.ResetTrigger("Grab");
        animator.ResetTrigger("BeginGrabbed");
        animator.ResetTrigger("UpTaunt");
        animator.ResetTrigger("SideTaunt");
        animator.ResetTrigger("DownTaunt");
        animator.ResetTrigger("UpSpecial");
        animator.ResetTrigger("NeutralSpecial");
        animator.ResetTrigger("SideSpecial");
        animator.ResetTrigger("DownSpecial");
    }

    // ---------------- UTILITY ----------------------------
    
    // Returns the Non-Game version of this character (for use in 3D scenes)
    public GameObject GetNonGameCharacter()
    {
        return nonGameCharacter;
    }

    // Returns the index of the character based on the characterIndices
    // dictionary as defined in the Awake() method of this function.
    // This int is used to determine what special moves to use in the animation controller.
    public int GetCharacterAnimIndex()
    {
        return characterIndices[gameObject.tag];
    }

    public CharacterAudioManager GetCharacterAudioManager()
    {
        return charAudio;
    }


    // ---------------- TEMPORARY RESET MECHANISM -------------------------
    private void OnSelect()
    {
        Destroy(GameObject.Find("SmashSettings"));
        SceneManager.LoadScene(1);
    }

}