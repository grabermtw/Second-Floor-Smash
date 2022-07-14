using System.Collections;
using UnityEngine;
using Cinemachine;

public class HazardRobo : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup targetGroup = default;
    private Rigidbody2D rb;
    [SerializeField]
    private float force;
    [SerializeField]
    private float forwardVelocity;
    [SerializeField]
    private float hitDamage, hitForce = 1f;

    [SerializeField]
    private ParticleSystem ps;
    [SerializeField]
    private float particleDuration = 1f;
    [SerializeField]
    private Transform flyPos, disablePos;
    private bool isFlying;

    [SerializeField]
    private float minDelay, maxDelay;
    private float timeUntilReset;
    private bool isActive;
    private Vector3 startingPos;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
        startingPos = transform.position;
    }

    private void Start()
    {
        DisableHazard();
        timeUntilReset = Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        if (!isActive)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            timeUntilReset -= Time.deltaTime;
            if (timeUntilReset <= 0)
            {
                timeUntilReset = Random.Range(minDelay, maxDelay);
                ResetHazard();
            }
        }
        else
        {
            rb.velocity = new Vector2(forwardVelocity, rb.velocity.y);
            if (transform.position.x > flyPos.position.x && !isFlying)
            {
                FlyAway();
            }
            if (transform.position.x > disablePos.position.x || transform.position.y < disablePos.position.y)
            {
                DisableHazard();
            }
        }
    }

    // Damage players who impact the hazard robo
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.TryGetComponent(out CharacterController cc))
        {
            // Don't damage a player who is shielding, ignore the "Shield" layer
            if (other.collider.gameObject.layer != 13) {
                cc.Strike(hitDamage, Mathf.PI / 4, hitForce, 1, true);
            }
        }
    }

    private void ResetHazard()
    {
        isActive = true;
        isFlying = false;
        rb.isKinematic = false;
        transform.position = startingPos;
        targetGroup.m_Targets[8].weight = 1f;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(force * Vector2.up);
        StartCoroutine(DoParticles(particleDuration));
    }

    private void FlyAway()
    {
        isFlying = true;
        targetGroup.m_Targets[8].weight = 0f;
        rb.AddForce(force * Vector2.up);
        StartCoroutine(DoParticles(particleDuration));
    }

    private void DisableHazard()
    {
        isActive = false;
        rb.isKinematic = true;
        targetGroup.m_Targets[8].weight = 0f;
    }

    private IEnumerator DoParticles(float duration)
    {
        ps.Stop();
        ps.Play();
        yield return new WaitForSeconds(duration);
        ps.Stop();
    }
}