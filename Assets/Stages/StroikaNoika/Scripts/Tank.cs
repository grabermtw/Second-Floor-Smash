using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    /*  the tank can be in only one of these states at a time */
    private enum State
    {
        idle,       // stationary
        driving,    // moving to destination
        arming,     // moving cannon toward stage
        tracking,   // following target
        disarming,  // moving cannon away from stage
        leaving,    // moving to exit
    };

    public float speed = 10f;       // speed modifier
    public float rotationSpeed;
    public float destination;       // where the tank drives to, x-
    
    public bool canShoot;           // if true, the tank will attempt to shoot a player

    public GameObject turret;       // reference to the tank turret
    public GameObject barrel;       // reference to the tank barrel
    public GameObject tip;          // reference to tip of tank barrel
    public GameObject explosion;    // reference to explosion prefab
    public Renderer[] renderers;
    
    private GameplayManager manager; // reference to the gameplay manager
    
    [SerializeField]private State state;            // current state of tank
    [SerializeField]private Transform target;       // the player the tank is aiming at
    // Start is called before the first frame update
    void Start()
    {
        state = State.driving;
        Destroy(gameObject, 45f);
    }

    public void Initialize(bool canShoot, Material mat, GameplayManager manager)
    {
        this.canShoot = canShoot;
        foreach(Renderer r in renderers)
        {
            r.material = mat;
        }
        this.manager = manager;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            // motion
            case State.driving :
                if (transform.position.x < destination)
                {
                    transform.Translate(speed * new Vector3(Time.deltaTime, 0, 0), Space.World);
                }
                else
                {
                    StartCoroutine(StateTransition(State.arming, 2f, 5f));
                }
                break;
            // swing cannon to side
            case State.arming :
                if (turret.transform.localEulerAngles.y < 90)
                {
                    float rotationAmount = 15 * Time.deltaTime;
                    if (turret.transform.localEulerAngles.y + rotationAmount > 90)
                    {
                        rotationAmount = 90 - turret.transform.localEulerAngles.y;
                    }
                    turret.transform.Rotate(0, rotationAmount, 0, Space.World);
                }
                else
                {
                    StartCoroutine(StateTransition(State.tracking, 0.1f, 1f));
                }
                break;
            // aiming barrel
            case State.tracking :
                if (target != null)
                {
                    Quaternion direction = Quaternion.Slerp(turret.transform.rotation, Quaternion.LookRotation(target.position - turret.transform.position), Time.deltaTime * rotationSpeed);
                    turret.transform.eulerAngles = new Vector3(0, direction.eulerAngles.y, 0);

                    direction = Quaternion.Slerp(barrel.transform.rotation, Quaternion.LookRotation(target.position + new Vector3(0,0.5f,0) - barrel.transform.position), Time.deltaTime * rotationSpeed);
                    barrel.transform.eulerAngles = new Vector3(direction.eulerAngles.x, barrel.transform.eulerAngles.y, 0);
                }
                else
                {
                    StartCoroutine(StateTransition(State.disarming, 0f, 1f));
                }
                break;
            // swing barrel to front
            case State.disarming :
                if (turret.transform.localEulerAngles.y > 0)
                {
                    float rotationAmount = -15 * Time.deltaTime;
                    if (turret.transform.localEulerAngles.y + rotationAmount < 0)
                    {
                        rotationAmount = -turret.transform.localEulerAngles.y;
                    }
                    turret.transform.Rotate(0, rotationAmount, 0, Space.World);                    
                }
                else
                {
                    ExitTank(); // get em outta here
                }
                break;                
        }
    }

    // shoots a projectile from the tank barrel, exploding at z=0 as well as damaging players
    private void Shoot()
    {   
        StateTransition(State.disarming, 1.0f, 2.0f);
        // project direction of tank barrel onto XY plane and get the position
        Plane xy = new Plane(Vector3.forward, Vector3.zero);
        Ray ray = new Ray(tip.transform.position, -tip.transform.up);
        float rayDist;
        Vector3 explosionPosition3D;
        if (xy.Raycast(ray, out rayDist))
        {
            explosionPosition3D = ray.GetPoint(rayDist);
            // create the explosion
            GameObject ex = Instantiate(explosion, explosionPosition3D, new Quaternion(0, 0, 0, 0));
            Destroy(ex, 2.2f);
            // damage players
            Collider2D[] buffer = new Collider2D[4];
            Vector2 explosionPosition2D = new Vector2(explosionPosition3D.x, explosionPosition3D.y);
            float radius = 1.5f;
            int numPlayers = Physics2D.OverlapCircleNonAlloc(explosionPosition2D, radius, buffer, 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12);
            for (int i = 0; i < numPlayers; i++)
            {
                int dir = buffer[i].transform.position.x > explosionPosition2D.x ? 1 : -1;
                float dist = Vector2.Distance(explosionPosition2D, buffer[i].transform.position);
                float angle = Mathf.Asin((buffer[i].transform.position.y - explosionPosition2D.y) / (dir * dist));
                
                buffer[i].transform.parent.GetComponent<CharacterController>().Strike(Mathf.Abs(25 * (1.5f - rayDist)), angle, 8 * (1.5f - rayDist), dir, true);
                Debug.Log("SHOT " + buffer[i].transform.parent.gameObject.name);
            }
        }
    }

    // sets the state to idle, waits a random amount of time (within a range), and then sets the state to the specified next state
    private IEnumerator StateTransition(State nextState, float minTime, float maxTime)
    {
        state = State.idle;
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        state = nextState;
        // if aiming now, choose a target
        if (state == State.tracking)
        {
            target = manager.ChooseRandomTarget();
            yield return new WaitForSeconds(Random.Range(4.5f, 6.5f));
            if (canShoot)
            {
                Shoot();
            }
            else
            {
                state = State.disarming;
            }
        }
    }

    // drives offstage and destroys the tank
    private void ExitTank()
    {
        destination += 50f;
        state = State.driving;
        Destroy(this.gameObject, 15f);
    }
}
