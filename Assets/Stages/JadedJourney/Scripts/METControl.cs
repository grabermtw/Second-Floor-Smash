using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class METControl : MonoBehaviour
{

    protected enum METState
    {
        Uninitialized,
        Hidden,
        Rising,
        Staring,
        Moving,
        AttackingClose,
        AttackingFar,
        Exiting,
        Sinking
    }

    protected enum METDebug
    {
        Off,
        AttackFar,
        AttackClose,
    }

    [SerializeField] 
    private METState state, nextState;
    [SerializeField]
    private METDebug debug;
    private float stateTimer = 0f;
    private float currWaitTime = 0f;

    public float minHiddenTime = 5f, maxHiddenTime = 30f;
    public float riseTime = 2f;
    public float minStareTime = 5f, maxStareTime = 15f;
    public float moveTime = 10f;
    public float exitTime = 15f;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private Quaternion initialRot;
    public float rotationResetSpeed = 8f;

    public Transform hiddenPos, risenPos, attacPos, exitPos;

    public int minAttacks = 2, maxAttacks = 5;
    public float attackDelay = 0.5f;
    private bool isAttackProcessing = false;

    public GameplayManager manager; // reference to the gameplay manager

    const float laserSpeed = 120f;
    const float aimSpeed = 6f;
    public GameObject projectile;
    public Transform[] cannons;
    public float range = 200;

    private AudioSource laserSource;
    public AudioClip creepyAudioClip;
    public float creepyAudioDelay = 1f;
    public float creepyAudioChance = 0.05f;
    private float creepyAudioTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        state = METState.Uninitialized;

        laserSource = GetComponentInChildren<AudioSource>();

        initialRot = transform.rotation;
        // target = GameObject.FindWithTag("Player").transform;
        // targetRb = target.GetComponent<Rigidbody>();
        // StartCoroutine(CannonControl());
    }

    // Update is called once per frame
    void Update()
    {
        bool isFirstTimeInState = state != nextState;
        state = nextState;

        stateTimer += Time.deltaTime;

        switch (state)
        {
            case (METState.Uninitialized):
            {
                // immediately transition to Hidden
                StateTransition();
                break;
            }
            case (METState.Hidden):
            {
                if (isFirstTimeInState)
                {
                    currWaitTime = Random.Range(minHiddenTime, maxHiddenTime);
                    transform.position = hiddenPos.position;
                    transform.rotation = initialRot;
                }
                if (stateTimer >= currWaitTime || debug != METDebug.Off)
                {
                    StateTransition();
                }
                break;
            }
            case (METState.Rising):
            {
                if (isFirstTimeInState)
                {
                    currWaitTime = riseTime;
                }
                //transform.position = Vector3.Slerp(hiddenPos.position, risenPos.position, stateTimer / currWaitTime);
                //transform.position = Vector3.SmoothDamp(transform.position, risenPos.position, ref velocity, riseTime - stateTimer);
                transform.position = SmoothLerp(hiddenPos.position, risenPos.position, stateTimer / currWaitTime);
                if (stateTimer >= currWaitTime)
                {
                    StateTransition();
                }
                break;
            }
            case (METState.Staring):
            {
                if (isFirstTimeInState)
                {
                    currWaitTime = Random.Range(minStareTime, maxStareTime);
                }
                if (stateTimer >= currWaitTime)
                {
                    StateTransition();
                }
                break;
            }
            case (METState.Moving):
            {
                if (isFirstTimeInState)
                {
                    currWaitTime = moveTime;
                }
                transform.position = SmoothLerp(risenPos.position, attacPos.position, stateTimer / currWaitTime);
                if (stateTimer >= currWaitTime)
                {
                    StateTransition();
                }
                break;
            }
            case (METState.AttackingClose):
            {
                // set up initial number of attacks and start coroutine
                if (isFirstTimeInState)
                {
                    StartCoroutine(CannonControl(1, false));
                }

                // when attacks are done, move to next
                if (!isAttackProcessing)
                {
                    StateTransition();
                }
                break;
            }
            case (METState.AttackingFar):
            {
                // set up initial number of attacks and start coroutine
                if (isFirstTimeInState)
                {
                    int numberOfAttacks = Random.Range(minAttacks, maxAttacks);
                    StartCoroutine(CannonControl(numberOfAttacks, false));
                }

                // when attacks are done, move to next
                if (!isAttackProcessing)
                {
                    StateTransition();
                }

                break;
            }
            case (METState.Exiting):
            {
                if (isFirstTimeInState)
                {
                    currWaitTime = exitTime;
                }
                //transform.position = Vector3.SmoothDamp(transform.position, exitPos.position, ref velocity, exitTime);
                transform.position = SmoothLerp(attacPos.position, exitPos.position, stateTimer / currWaitTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRot, Time.deltaTime * rotationResetSpeed);
                if (stateTimer >= currWaitTime)
                {
                    StateTransition();
                }
                break;
            }
            case (METState.Sinking):
            {
                if (isFirstTimeInState)
                {
                    currWaitTime = riseTime;
                }
                transform.position = SmoothLerp(risenPos.position, hiddenPos.position, stateTimer / currWaitTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRot, Time.deltaTime * rotationResetSpeed);
                if (stateTimer >= currWaitTime)
                {
                    StateTransition();
                }
                break;
            }
        }
        if (isFirstTimeInState)
        {
            isFirstTimeInState = false;
        }

        // do creepy audio
        if (state != METState.Uninitialized && state != METState.Hidden)
        {
            creepyAudioTimer -= Time.deltaTime;
            if (creepyAudioTimer <= 0)
            {
                creepyAudioTimer = creepyAudioDelay;
                float roll = Random.Range(0f, 1f);
                if (roll < creepyAudioChance)
                {
                    Debug.Log("Playing creepy audio");
                    laserSource.PlayOneShot(creepyAudioClip);
                }
            }
        }
    }

    private void StateTransition()
    {
        stateTimer = 0f;
        switch (state)
        {
            case (METState.Uninitialized):
            {
                // go to hidden 
                nextState = METState.Hidden;
                break;
            }
            case (METState.Hidden):
            {
                // go to rising   
                nextState = METState.Rising;
                break;
            }
            case (METState.Rising):
            {
                // either move forward to attack at close range
                // or stay back and attack from a distance
                bool isClose = Random.Range(0, 2) == 0;

                if ((isClose || debug == METDebug.AttackClose) && debug != METDebug.AttackFar)
                {
                    nextState = METState.Moving;
                }
                else
                {
                    nextState = METState.AttackingFar;
                }
                break;
            }
            case (METState.Moving):
            {
                // arrived, start attacking
                nextState = METState.AttackingClose;
                break;
            }
            case (METState.AttackingClose):
            {
                // exit
                nextState = METState.Exiting;
                break;
            }
            case (METState.AttackingFar):
            {
                nextState = METState.Sinking;
                break;
            }
            case (METState.Exiting):
            {
                nextState = METState.Hidden;
                break;
            }
            case (METState.Sinking):
            {
                nextState = METState.Hidden;
                break;
            }
        }
    }

    private void LaserAttack(int numAttacks)
    {
        
    }

    // Handle aiming the cannon
    private IEnumerator CannonControl(int numAttacks, bool isSingleTarget)
    {
        Debug.Log("Attacking " + numAttacks + " times ( " + (isSingleTarget ? "same" : "multi") + " target)");
        isAttackProcessing = true;
        Transform target = manager.ChooseRandomTarget();
        for (int i = 0; i < numAttacks; i++)
        {            
            // pick new target if we don't want to attack the same target every time
            if (i > 0 && !isSingleTarget)
            {
                target = manager.ChooseRandomTarget();
            }

            if (Vector3.Distance(target.position, transform.position) <= range)
            {
                if (target.TryGetComponent<Rigidbody2D>(out Rigidbody2D targetRb))
                {
                    // Determine how long to aim for before firing the cannon
                    float aimTime = Random.Range(1, 6);
                    float currentAimTime = 0;
                    while (currentAimTime < aimTime && targetRb != null)
                    {
                        // Estimate the target's future position by the time the laser reaches them based on their velocity and the laser's speed
                        Vector3 velocity = new Vector3(targetRb.velocity.x, targetRb.velocity.y, 0);
                        Vector3 targetPosition = target.position + velocity * Vector3.Distance(target.position, transform.position) / laserSpeed;
                        // Calculate where to aim
                        Quaternion direction = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), Time.deltaTime * aimSpeed);
                        // Clamp the rotation so that the barrel doesn't clip through the platform
                        if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
                        {
                            transform.rotation = direction;
                        }
                        foreach (Transform cannon in cannons)
                        {
                            // Estimate the target's future position by the time the laser reaches them based on their velocity and the laser's speed
                            targetPosition = target.position + velocity * Vector3.Distance(target.position, cannon.position) / laserSpeed;
                            // Calculate where to aim
                            direction = Quaternion.Slerp(cannon.rotation, Quaternion.LookRotation(targetPosition - cannon.position), Time.deltaTime * aimSpeed);
                            // Clamp the rotation so that the barrel doesn't clip through the platform
                            if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
                            {
                                cannon.rotation = direction;
                            }
                        }
                        //  transform.LookAt(target);
                        currentAimTime += Time.deltaTime;
                        yield return null;
                    }
                    // Fire the cannon
                    foreach (Transform cannon in cannons)
                    {
                        GameObject laser = Instantiate(projectile, 2 * cannon.forward + cannon.position, cannon.rotation);
                        laser.GetComponent<LaserBolt>().SetSpeed(laserSpeed);
                    }
                    // play the sound
                    if (laserSource != null)
                    {
                        laserSource.Play();
                    }
                }
            }
            yield return new WaitForSeconds(attackDelay);
        }
        isAttackProcessing = false;
    }

    public static Vector3 SmoothLerp(Vector3 from, Vector3 to, float t)
    {
        float x = Mathf.SmoothStep(from.x, to.x, t);
        float y = Mathf.SmoothStep(from.y, to.y, t);
        float z = Mathf.SmoothStep(from.z, to.z, t);

        return new Vector3 (x, y, z);
    }
}
