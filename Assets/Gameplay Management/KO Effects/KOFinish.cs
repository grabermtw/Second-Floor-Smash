using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOFinish : MonoBehaviour
{
    // Called when the particle system finishes its thing
    public void OnParticleSystemStopped()
    {
        PlayerNumberManager numManage = GameObject.Find("PlayerInputManager").GetComponent<PlayerNumberManager>();
        numManage.Spawn(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) - 1, false);
        Destroy(gameObject);
    }
}
