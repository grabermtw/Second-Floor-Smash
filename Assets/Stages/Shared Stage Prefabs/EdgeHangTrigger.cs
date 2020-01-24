using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeHangTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure that the thing entering the trigger is actually a player
        if(other.gameObject.layer >= 9 && other.gameObject.layer <= 12)
        {
            Debug.Log("Entered edge trigger");
            CharacterController charCrtl = other.gameObject.transform.parent.gameObject.GetComponent<CharacterController>();
            Rigidbody2D rb = charCrtl.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0,0);
            rb.isKinematic = true;
            if(gameObject.CompareTag("LeftEdge")){
                charCrtl.Hang(-1, transform.position);
            }
            else{
                charCrtl.Hang(1, transform.position);
            }
        }
    }
}
