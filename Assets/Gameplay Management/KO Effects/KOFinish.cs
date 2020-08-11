using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOFinish : MonoBehaviour
{
    // Called when the particle system finishes its thing
    public void OnParticleSystemStopped()
    {
        // Find the GameplayManager and call its FinishKO method to handle whether to respawn
        GameplayManager gameManage = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        gameManage.FinishKO(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) - 1);
        Destroy(gameObject);
    }
}
