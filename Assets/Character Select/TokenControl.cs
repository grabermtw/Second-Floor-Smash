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

    // Change the tag of the selected character to change their appearance
    public void ChangeSkin()
    {
        switch(currentCharacter)
        {
            case null:
                break;
            case "Calvin Crunkleton":
                currentCharacter = "Calvin Crunkleton Shirtless";
                Debug.Log("Calvin lost his shirt!");
                // Reselect Calvin so that he's now shirtless
                SetHeld(true);
                SetHeld(false);
                break;
            case "Calvin Crunkleton Shirtless":
                currentCharacter = "Calvin Crunkleton";
                Debug.Log("Calvin found his shirt!");
                // Reselect Calvin so that he's no longer shirtless
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers":
                currentCharacter = "Nate Rogers ShortHair";
                Debug.Log("Nate cut his hair!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers ShortHair":
                currentCharacter = "Nate Rogers";
                Debug.Log("Nate grew out his hair!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Tim Henderson":
                currentCharacter = "Tim Tastings";
                Debug.Log("Welcome back to Tim's Tastings");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Tim Tastings":
                currentCharacter = "Tim Tastings Hat";
                Debug.Log("Welcome back to Tim's Tastings, today we have a hat");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Tim Tastings Hat":
                currentCharacter = "Tim Henderson";
                Debug.Log("That's all for today, come back next week when I'll be tasting... who knows what?");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jasmine Lim":
                currentCharacter = "Jasmine Lim Casual";
                Debug.Log("Jasmine got out of work!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jasmine Lim Casual":
                currentCharacter = "Jasmine Lim";
                Debug.Log("Jasmine went to work!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jordan Woo":
                currentCharacter = "Jordan Woo Banana";
                Debug.Log("Jordan has his banana shirt on!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jordan Woo Banana":
                currentCharacter = "Jordan Woo";
                Debug.Log("Jordan isn't wearing his banana shirt anymore.");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Tom Zong":
                currentCharacter = "Tom Zong Shades";
                Debug.Log("Tom is wearing shades!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Tom Zong Shades":
                currentCharacter = "Tom Zong";
                Debug.Log("Tom is no longer wearing shades.");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Fred Delawie":
                currentCharacter = "Fred Delawie Uniform";
                Debug.Log("Fred has joined ROTC!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Fred Delawie Uniform":
                currentCharacter = "Fred Delawie";
                Debug.Log("Fred finished ROTC!");
                SetHeld(true);
                SetHeld(false);
                break;
            default:
                break;
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
