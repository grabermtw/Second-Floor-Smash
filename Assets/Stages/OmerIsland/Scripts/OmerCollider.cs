using UnityEngine;

public class OmerCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        CharacterController cc = other.transform.GetComponent<CharacterController>();
        if (cc != null)
        {
            int dir = transform.position.x > other.transform.position.x ? -1 : 1;
            cc.Strike(15, Mathf.PI / 4,  0.25f, dir, true);
        }
    }    
}