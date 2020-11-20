using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCControl : MonoBehaviour
{
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        // Check what our situation is to determine our behavior
        switch(SceneManager.GetActiveScene().name)
        {
            case "Opening":
                anim.SetTrigger("Opening");
            break;

            case "Hub":
                // Are we in the character select or are we an NPC?
                // NPC
                if (transform.parent == null || !transform.parent.name.Contains("Podium"))
                {
                    anim.SetTrigger("Hub");
                }
                else // Character Select
                {
                    anim.SetTrigger("CharacterSelect");
                }
            break;

            default:
                Debug.LogWarning("What the heck is this scene? " + gameObject.name + " doesn't know what to do here!");
                anim.SetTrigger("Opening");
            break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
