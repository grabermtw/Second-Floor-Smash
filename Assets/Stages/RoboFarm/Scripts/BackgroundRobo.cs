using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackgroundRobo : MonoBehaviour
{
    private NavMeshAgent agent;
    public float rotationSpeed = 10f;
    private Terrain terrain;
    public bool big = false;

    private float minX = -100, maxX = 100, minZ = 0, maxZ = 200;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        Vector3 center = terrain.transform.position + terrain.terrainData.bounds.center;
        Vector3 maxBounds = center + terrain.terrainData.bounds.extents;
        Vector3 minBounds = center - terrain.terrainData.bounds.extents;
        minX = minBounds.x;
        minZ = minBounds.z;
        maxX = maxBounds.x;
        maxZ = maxBounds.z;

        float minScale = 0.5f, maxScale = 1.5f;
        if (big)
        {
            minScale = 25f;
            maxScale = 40f;
        }
        transform.localScale = new Vector3(Random.Range(minScale, maxScale), Random.Range(minScale, maxScale), Random.Range(minScale, maxScale));
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Done
                    agent.SetDestination(GetRandomPos());
                }
            }
        }
        Quaternion rot = Quaternion.LookRotation(agent.velocity.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

    }

    private Vector3 GetRandomPos()
    {
        Vector3 pos = new Vector3(0,20,0);
        bool foundPos = false;
        UnityEngine.AI.NavMeshHit hit;
        int i = 0;
        while (!foundPos && i < 100)
        {
            i++;
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
            randomPos.y = terrain.terrainData.GetHeight((int)randomPos.x, (int)randomPos.y);
            foundPos = UnityEngine.AI.NavMesh.SamplePosition(randomPos, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas);
            if (foundPos)
            {
                pos = hit.position;
            }
        }
        return pos;
    }
}
