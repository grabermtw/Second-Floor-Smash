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
    SmashSettings playerCharList;
    CharacterList characterList;
    int playerNum;
    private GameObject[] currentCharacter; //This is the array of the selected character's skins
    private string currCharTag = ""; // This is the name of the character (like the actual name, not the skin's name)
    private int currSkin = 0; // This is the index of the currently selected skin in the character's skin array
    private GameObject podium; // The podium in the character select screen upon which our selected character will stand
    private Vector3 podPos; // This is the standard position of the podium
    private float heightAdjust; // This is the amount that the character's podium's height should be adjusted
    private GameObject charPreview; // This will be the character that's currently on our podium


    // Start is called before the first frame update
    void Start()
    {

        // Assign our player number
        playerNum = int.Parse(gameObject.tag.Substring(gameObject.tag.IndexOf(" ")));

        // Get ready for some raycasting!
        GameObject canvas = GameObject.Find("CharacterSelectCanvas");
        gr = canvas.GetComponent<GraphicRaycaster>();

        // Find the appropriate player podium
        podium = GameObject.Find("P" + playerNum + " Podium");
        podPos = podium.GetComponent<PodiumControl>().GetInitialLocalPosition();

        // Find the CharacterList, which has all the characters and their skins
        characterList = GameObject.Find("CharacterManager").GetComponent<CharacterList>();

        // Find the player character list for assigning our character selections
        playerCharList = GameObject.Find("SmashSettings").GetComponent<SmashSettings>();

        Debug.Log(gameObject.tag + " is currently using " + Gamepad.current);
    }

    // Update is called once per frame
    void Update()
    {
        if (held)
        {
            

        }
    }

    // Change the tag of the selected character to change their appearance
    public void ChangeSkin()
    {
        currSkin++;
        if (currSkin >= currentCharacter.Length)
        {
            currSkin = 0;
        }
        SetHeld(true);
        SetHeld(false);

    }

    // Update whether we are being held by the hand
    public void SetHeld(bool hold)
    {
        held = hold;
        if (!hold)
        {
            //Transform newParent = null;

            // Do the raycast
            pointEventData.position = Camera.main.WorldToScreenPoint(transform.position);
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointEventData, results);

            // Analyze the results
            if (results.Count > 1)
            {
                foreach (RaycastResult result in results)
                {
                    // We've got an icon!
                    if (result.gameObject.layer == 16 && currCharTag != result.gameObject.tag)
                    {
                        // This is our current character
                        currCharTag = result.gameObject.tag;
                        currSkin = 0;
                        currentCharacter = characterList.GetCharacterArray(currCharTag);
                        heightAdjust = characterList.GetHeightOffset(currCharTag);
                        Debug.Log(currentCharacter);

                       // newParent = result.gameObject.transform;
                        break;
                    }
                }
            }
            if (currentCharacter != null)
            {
                
                // Set the new parent
                //transform.SetParent(newParent);

                // Reset the podium
                podium.GetComponent<PodiumControl>().ResetRotation();

                // Select the current character
                playerCharList.ChooseCharacter(currentCharacter[currSkin], playerNum);
                charPreview = Instantiate(currentCharacter[currSkin], podium.transform);
                
                // Let the podium know who it's got on it
                podium.GetComponent<PodiumControl>().AssignCurrentCharacter(charPreview);

                // Change the height of the podium so that the character isn't blocked by/blocking the icons
                podium.transform.localPosition = podPos + new Vector3(0, heightAdjust, 0);

                // Change and disable a bunch of things on the instantiated character so that it doesn't break everything
                charPreview.transform.localPosition = new Vector3(0, 0, 0);
                charPreview.transform.eulerAngles = new Vector3(0, -90, 0);
                charPreview.GetComponent<CharacterController>().enabled = false;
                charPreview.GetComponent<DamageControl>().enabled = false;
                charPreview.GetComponent<Grab>().enabled = false;
                charPreview.GetComponent<Grabbed>().enabled = false;
                charPreview.GetComponent<PlayerInput>().enabled = false;
                charPreview.transform.Find("StandingCollider").transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }
        else
        {
            //currentCharacter = null;

            // Deselect the character by setting it as null
            playerCharList.ChooseCharacter(null, playerNum);

            // Remove the preview character from the podium
            Destroy(charPreview);

        }
    }

}
