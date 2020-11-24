using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : MonoBehaviour
{
    public float roamingRadius = 10;
    public float roamingTimerMax = 10f;
    public float roamingTimerMin = 3f;
    // DEBUG, leave this empty for regular behavior
    public Transform destination;

    private Animator anim;
    private NavMeshAgent nav;
    private Transform target;
    private float timer;
    private float currentRoamTime;
    private bool isRoaming;
    
    
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        timer = roamingTimerMax;
        currentRoamTime = roamingTimerMin;
        transform.Find("Body").localEulerAngles = new Vector3(0,0,0);
        StartCoroutine(Roaming());

        // Calculate the base offset adjustment using the capsule collider
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        nav.baseOffset = - (capsule.center.y - capsule.height / 2) - 0.05f;
    }
 
    // Update is called once per frame
    void Update ()
    {
        timer += Time.deltaTime;
        
        // Calculate angular velocity
        Vector3 s = transform.InverseTransformDirection(nav.velocity).normalized;
        float turn = s.x;

        // let the animator know what's going on
        anim.SetFloat("Speed", nav.velocity.magnitude);
        anim.SetFloat("Turn", turn * 2);

        // When time is up, roam to a new destination
        if (timer >= currentRoamTime) {
            Vector3 newPos;
            // debug purposes
            if (destination == null)
            {
                newPos = GetNewDestination(transform.position, roamingRadius, -1);
            }
            else
            {
                newPos = destination.position;
            }
            nav.SetDestination(newPos);
            currentRoamTime = Random.Range(roamingTimerMin, roamingTimerMax);
            timer = 0;
        }
    }

    IEnumerator Roaming()
    {
        while (true)
        {
            // Wait till we're at a door
            yield return new WaitUntil(() => nav.isOnOffMeshLink);

            OffMeshLinkData data = nav.currentOffMeshLinkData;

            // data.offMeshLink should never be null... and yet sometimes it is...
            if (data.offMeshLink != null)
            {
                Door door = data.offMeshLink.GetComponent<Door>();
                Debug.Log(door.gameObject);
                
                // Open door
                door.OpenDoor();
                
                // Face the correct direction
                transform.LookAt(data.endPos);

                // Wait until it is open
                yield return new WaitUntil(() => door.currentState == Door.State.Open);
                
                // Walk through the door
                yield return StartCoroutine(Walk(data));
                
                nav.CompleteOffMeshLink();
                // Close the door
                door.CloseDoor();
            }
            else // This shouldn't be needed but for some reason it is.
            {
                Debug.LogWarning(gameObject.name + " encountered a null offMeshLink for no reason!");
                nav.CompleteOffMeshLink();
            }
        }
    }


    // Simply interpolate a straight line from start to end.
    IEnumerator Walk(OffMeshLinkData data)
    {
        float timeLeft = Vector3.Distance(data.offMeshLink.startTransform.position, data.offMeshLink.endTransform.position) * (1 / nav.speed) + 1;
        float passTime = timeLeft;

        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + nav.baseOffset * Vector3.up;

        Vector3 lookPos = endPos - startPos;
        lookPos.y = 0;

        while (timeLeft > 0)
        {
            transform.position = Vector3.Lerp(endPos, startPos, timeLeft / passTime);
            anim.SetFloat("Speed", nav.speed - 0.2f);

            timeLeft -= Time.deltaTime;

            yield return null;
        }
    }


    public static Vector3 GetNewDestination(Vector3 origin, float dist, int layermask) {
        // Get a random new destination
        Vector3 newDirection = Random.insideUnitSphere * dist;
 
        newDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition(newDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}
