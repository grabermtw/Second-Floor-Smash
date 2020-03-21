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
                currentCharacter = "Calvin Crunkleton Red";
                Debug.Log("Calvin got red shorts!");
                // Reselect Calvin so that he's no longer shirtless
                SetHeld(true);
                SetHeld(false);
                break;
            case "Calvin Crunkleton Red":
                currentCharacter = "Calvin Crunkleton Red Shirtless";
                Debug.Log("Calvin lost his shirt!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Calvin Crunkleton Red Shirtless":
                currentCharacter = "Calvin Crunkleton Rio";
                Debug.Log("Ya boi went to Rio!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Calvin Crunkleton Rio":
                currentCharacter = "Calvin Crunkleton";
                Debug.Log("Coronavirus came and sent Calvin back home.");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers":
                currentCharacter = "Nate Rogers LongHair";
                Debug.Log("Nate grew his hair!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers LongHair":
                currentCharacter = "Nate Rogers ShortHair";
                Debug.Log("Nate cut his hair!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers ShortHair":
                currentCharacter = "Nate Rogers LongestHair";
                Debug.Log("Nate grew out his hair more! this is freshman year");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers LongestHair":
                currentCharacter = "Nate Rogers Beau";
                Debug.Log("there aint no ghost in this hallway");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Nate Rogers Beau":
                currentCharacter = "Nate Rogers";
                Debug.Log("present nate");
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
                currentCharacter = "Jordan Woo Ghost";
                Debug.Log("I am the Ghost of Prince Frederick");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jordan Woo Ghost":
                currentCharacter = "Jordan Woo";
                Debug.Log("This body is failing me.");
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
            case "Joy London":
                currentCharacter = "Joy London Jacket";
                Debug.Log("Joy put on a jacket!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Joy London Jacket":
                currentCharacter = "Joy London";
                Debug.Log("Joy took off her jacket!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Flo Ning":
                currentCharacter = "Flo Ning Haircut";
                Debug.Log("Flo cut her hair!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Flo Ning Haircut":
                currentCharacter = "Flo Ning";
                Debug.Log("Flo grew out her hair!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Bebo Harraz":
                currentCharacter = "Bebo Harraz Glasses";
                Debug.Log("Bebo put on glasses!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Bebo Harraz Glasses":
                currentCharacter = "Bebo Harraz";
                Debug.Log("Bebo took off his glasses!");
                SetHeld(true);
                SetHeld(false);
                break;
            case "AJ Shannon":
                currentCharacter = "AJ Shannon Sweatshirt";
                Debug.Log("AJ sweatshirt (boutta battle tim)");
                SetHeld(true);
                SetHeld(false);
                break;
            case "AJ Shannon Sweatshirt":
                currentCharacter = "AJ Shannon Tee";
                Debug.Log("AJ Tee");
                SetHeld(true);
                SetHeld(false);
                break;
            case "AJ Shannon Tee":
                currentCharacter = "AJ Shannon Tank";
                Debug.Log("AJ Tank");
                SetHeld(true);
                SetHeld(false);
                break;
            case "AJ Shannon Tank":
                currentCharacter = "AJ Shannon Buttondown";
                Debug.Log("AJ Buttondown");
                SetHeld(true);
                SetHeld(false);
                break;
            case "AJ Shannon Buttondown":
                currentCharacter = "AJ Shannon";
                Debug.Log("AJ fratty");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Amanda OShaughnessy":
                currentCharacter = "Amanda OShaughnessy V2";
                Debug.Log("Amanda V2");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Amanda OShaughnessy V2":
                currentCharacter = "Amanda OShaughnessy";
                Debug.Log("Amanda");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Anders Julin":
                currentCharacter = "Anders Sweatshirt";
                Debug.Log("Anders Sweatshirt");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Anders Sweatshirt":
                currentCharacter = "Anders Casual";
                Debug.Log("Anders Casual");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Anders Casual":
                currentCharacter = "Anders XR";
                Debug.Log("Anders XR");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Anders XR":
                currentCharacter = "Anders Freshman";
                Debug.Log("Anders Freshman");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Anders Freshman":
                currentCharacter = "Anders Julin";
                Debug.Log("Anders Julin");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Christina Huang":
                currentCharacter = "Christina Huang Bangs";
                Debug.Log("Christina Bangs");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Christina Huang Bangs":
                currentCharacter = "Christina Huang";
                Debug.Log("Christina sans Bangs");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Graber":
                currentCharacter = "Graber Syr";
                Debug.Log("Graber Syr");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Graber Syr":
                currentCharacter = "Graber TMM Hat";
                Debug.Log("Graber TMM Hat");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Graber TMM Hat":
                currentCharacter = "Graber TMM";
                Debug.Log("Graber TMM");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Graber TMM":
                currentCharacter = "Graber TMM 2019";
                Debug.Log("Graber TMM 2019");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Graber TMM 2019":
                currentCharacter = "Graber";
                Debug.Log("Graber gray shirt");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jesse Bent Metal":
                currentCharacter = "Jesse Sweatshirt";
                Debug.Log("Jessse sweatshirt");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jesse Sweatshirt":
                currentCharacter = "Jesse Longsleeves";
                Debug.Log("Jessse longsleeves");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jesse Longsleeves":
                currentCharacter = "Jesse Parreira";
                Debug.Log("Jessse Parreira");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jesse Parreira":
                currentCharacter = "Jesse XR";
                Debug.Log("Jessse XR");
                SetHeld(true);
                SetHeld(false);
                break;
            case "Jesse XR":
                currentCharacter = "Jesse Bent Metal";
                Debug.Log("Jessse Bent Metal");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball":
                currentCharacter = "John Ball Adinkra";
                Debug.Log("John adinkra");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball Adinkra":
                currentCharacter = "John Ball Adinkra Shades";
                Debug.Log("John adinkra shades");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball Adinkra Shades":
                currentCharacter = "John Ball XR";
                Debug.Log("John XR");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball XR":
                currentCharacter = "John Ball Bent Metal";
                Debug.Log("John bent metal");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball Bent Metal":
                currentCharacter = "John Ball Desmesmert";
                Debug.Log("John desmesmert");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball Desmesmert":
                currentCharacter = "John Ball Freshman";
                Debug.Log("John freshman");
                SetHeld(true);
                SetHeld(false);
                break;
            case "John Ball Freshman":
                currentCharacter = "John Ball";
                Debug.Log("John ballll");
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
