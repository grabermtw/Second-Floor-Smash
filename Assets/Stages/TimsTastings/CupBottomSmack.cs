using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBottomSmack : MonoBehaviour
{
    // When Tim is putting his cup down, hurt anyone who is hit by the cup
    private void OnCollisionEnter2D(Collision2D other)
    {
        try
        {
            int dir = (transform.position.x > other.transform.position.x ? -1 : 1);
            other.gameObject.GetComponent<CharacterController>().Strike(20, -10, 0.3f, dir);
        }
        catch
        {
            
        }
    }
}
