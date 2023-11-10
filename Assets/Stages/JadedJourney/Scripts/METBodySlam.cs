using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class METBodySlam : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        CharacterController cc = other.transform.GetComponent<CharacterController>();
        if (cc != null)
        {
            int dir = transform.position.x > other.transform.position.x ? -1 : 1;
            cc.Strike(15, Mathf.PI / 4,  0.35f, dir, true);
        }
    }
}
