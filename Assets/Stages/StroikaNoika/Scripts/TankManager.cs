using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{

    public float chanceToShoot = 0.2f;
    public float timeToSpawn = 20f;

    private float currTime;

    public GameObject tankPrefab;
    public GameObject tankSpawn;
    public Material[] materials;
    public GameplayManager manager;

    // Start is called before the first frame update
    void Start()
    {
        currTime = timeToSpawn;
        SpawnTank();
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime <= 0f)
        {
            SpawnTank();
            currTime = timeToSpawn;
        }
        currTime -= Time.deltaTime;
    }

    private void SpawnTank()
    {
        bool canShoot = false;
        float i = Random.Range(0f, 1f);
        if (i < chanceToShoot)
        {
            canShoot = true;
        }
        Material mat = materials[Random.Range(0,3)];
        GameObject tank = Instantiate(tankPrefab, tankSpawn.transform);
        tank.GetComponent<Tank>().Initialize(canShoot, mat, manager);
    }
}
