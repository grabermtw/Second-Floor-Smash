using System.Collections;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    
    const float impactVelMax = 30f;
    private Rigidbody rb;
    private float speed;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyIfMissed());
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        CharacterController cc = other.transform.GetComponent<CharacterController>();
        if (cc != null)
        {
            int dir = transform.position.x > other.transform.position.x ? -1 : 1;
            cc.Strike(15, Mathf.PI / 4,  0.25f, dir, true);
        }
    }

    // Destroy the laser if we very clearly missed the target.
    private IEnumerator DestroyIfMissed()
    {
        yield return new WaitForSeconds(8);
        Debug.Log("Bolt destoryed bc missed");
        Destroy(gameObject);
    }

    
}
