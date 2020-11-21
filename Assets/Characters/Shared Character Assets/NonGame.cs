using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class NonGame : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent nav;
    private NPCControl npcControl;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        npcControl = GetComponent<NPCControl>();

        // Check what our situation is to determine our behavior
        switch(SceneManager.GetActiveScene().name)
        {
            case "Opening":
                anim.SetTrigger("Opening");
                nav.enabled = false;
                npcControl.enabled = false;
            break;

            case "Hub":
                // Are we in the character select or are we an NPC?
                // NPC
                if (transform.parent == null || !transform.parent.name.Contains("Podium"))
                {
                    anim.SetTrigger("Hub");
                    nav.enabled = true;
                    npcControl.enabled = true;
                }
                else // Character Select
                {
                    anim.SetTrigger("CharacterSelect");
                    nav.enabled = false;
                    npcControl.enabled = false;
                }
            break;

            default:
                Debug.LogWarning("What the heck is this scene? " + gameObject.name + " doesn't know what to do here!");
                anim.SetTrigger("Opening");
                nav.enabled = false;
                npcControl.enabled = false;
            break;
        }
        
    }
}
