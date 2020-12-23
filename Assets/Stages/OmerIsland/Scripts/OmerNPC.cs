using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OmerNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    private static NavPoint[] navPoints;
    private Animator anim;
    private int navPoint = -1;

    private GameObject obj2D;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        obj2D = transform.GetChild(1).gameObject;
        agent = GetComponent<NavMeshAgent>();
        if (navPoints == null)
        {
            navPoints = GameObject.FindObjectsOfType<NavPoint>();
        }
        SetDestination();
        obj2D.SetActive(false);
    }

    private void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
        // check if we reached destination
        if (navPoint != -1 && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    StartCoroutine(ReachedDestination());
                }
            }    
        }
    }

    private void LateUpdate()
    {
        obj2D.transform.rotation = Quaternion.identity;    
    }

    void SetDestination()
    {
        navPoint = -1;
        int i = Random.Range(0, navPoints.Length);
        NavPoint destination = navPoints[i];
        // make sure we are moving and the destinaton is not taken by another Omer
        if (i != navPoint && !destination.isTaken)
        {
            agent.SetDestination(destination.transform.position);
            navPoint = i;
        }
        else
        {
            SetDestination();
        }
    }

    IEnumerator ReachedDestination()
    {
        //Debug.Log("reached");
        float i = Random.Range(0f, 10f);
        yield return new WaitForSeconds(i);

        SetDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        obj2D.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        obj2D.SetActive(false);
    }
}
