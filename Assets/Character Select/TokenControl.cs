using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TokenControl : MonoBehaviour
{
    private GraphicRaycaster gr;
    private PointerEventData pointEventData = new PointerEventData(null);
    bool held = true;
    PlayerCharList playerCharList;
    int playerNum;
    string currentCharacter = null;

    // Start is called before the first frame update
    void Start()
    {
        // Get ready for some raycasting!
        GameObject canvas = GameObject.Find("Canvas");
        gr = canvas.GetComponent<GraphicRaycaster>();

        // Find the player character list for assigning our character selections
        playerCharList = GameObject.Find("PlayerCharacterList").GetComponent<PlayerCharList>();

        // Assign our player number
        playerNum = int.Parse(gameObject.tag.Substring(gameObject.tag.IndexOf(" ")));

        Debug.Log(gameObject.tag + " is currently using " + Gamepad.current);
    }

    // Update is called once per frame
    void Update()
    {
        if (held)
        {
            // Do the raycast
            pointEventData.position = transform.position;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointEventData, results);

            // Analyze the results
            if (results.Count > 1)
            {
                foreach (RaycastResult result in results)
                {
                    // We've got an icon!
                    if (result.gameObject.layer == 16)
                    {
                        // This is our current character
                        currentCharacter = result.gameObject.tag;
                        Debug.Log(currentCharacter);
                        break;
                    }
                }
            }

        }
    }

    // Update whether we are being held by the hand
    public void SetHeld(bool hold)
    {
        held = hold;
        if(!hold)
        {
            // Select the current character
            playerCharList.ChooseCharacter(currentCharacter, playerNum);
        }
        else{
            // Deselect the character by setting it as null
            playerCharList.ChooseCharacter(null, playerNum);
        }
    }

}
