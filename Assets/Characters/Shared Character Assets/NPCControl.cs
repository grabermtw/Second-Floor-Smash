using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : MonoBehaviour
{
    public float roamingRadius = 10;
    public float roamingTimer = 8f;

    private Animator anim;
    private NavMeshAgent nav;
    private Transform target;
    private float timer;
    
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        timer = roamingTimer;
        transform.Find("Body").localEulerAngles = new Vector3(0,0,0);
    }
 
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
 
        // When time is up, roam to a new destination
        if (timer >= roamingTimer) {
            Vector3 newPos = GetNewDestination(transform.position, roamingRadius, -1);
            nav.SetDestination(newPos);
            timer = 0;
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
