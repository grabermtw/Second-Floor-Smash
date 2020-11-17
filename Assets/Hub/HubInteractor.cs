using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HubInteractor : MonoBehaviour
{
    public string message;
    public bool interactable;
    public float distanceToPlayer;
    private Transform player;
    protected FirstPersonControl playerControl;
    private bool playerNear = false;


    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        Debug.Log(player.gameObject);
        playerControl = player.gameObject.GetComponent<FirstPersonControl>();
    }
    
    protected virtual void Update()
    {
        if (interactable && player != null)
        {
            // If the player is in range, tell it to use this door
            if ((player.position - transform.position).magnitude <= distanceToPlayer)
            {
                playerNear = true;
                playerControl.SetInteract(this, message);
                
            }
            else
            {
                if(playerNear){
                    // Out of range so not using this door anymore.
                    playerControl.UnsetInteract();
                    playerNear = false;
                }
            }
        }
    }

    public abstract void Interact();
}
