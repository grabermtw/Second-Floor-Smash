using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectControl : MonoBehaviour
{
    private Vector2 leftJoystick; // holds the directions that the left joystick is pointing
    private float trackingSpeed = 3.5f; // how fast should the cursor move
    Transform token; // The token we'll use to select our character
    Transform cursor; // Our cursor
    Transform canvas;
    bool ready = false; // Make sure everything's set before taking input
    PlayerNumberCharacterSelect playerManager;
    SmashSettings playerCharList;
    TokenControl tokCtrl;
    bool hold = true;
    private PointerEventData pointEventData = new PointerEventData(null);
    private GraphicRaycaster gr;
    private int playerNum;
    private PodiumControl podium;
    private CharToStageSelect charToStageSelect;


    void Awake()
    {
        canvas = GameObject.Find("CharacterSelectCanvas").transform;
        transform.SetParent(canvas);
        transform.rotation = canvas.parent.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the appropriate cursor and token
        playerManager = GameObject.Find("PlayerInputManager").GetComponent<PlayerNumberCharacterSelect>();
        token = Instantiate(playerManager.GetPlayerCursor(), transform).transform;
        cursor = token.GetComponentsInChildren<Transform>()[1];
        tokCtrl = token.gameObject.GetComponent<TokenControl>();

        // Prepare the graphics raycaster so we can click on things
        gr = canvas.GetComponent<GraphicRaycaster>();

        // Get the player number
        playerNum = int.Parse(token.gameObject.tag.Substring(token.tag.Length - 1));
        transform.localPosition = new Vector2((playerNum - 1) * 400 + -645, -300);

        // Find the player character list for checking whether we can begin the game in OnStart
        playerCharList = GameObject.Find("SmashSettings").GetComponent<SmashSettings>();

        playerCharList.AddPlayer(GetComponent<PlayerInput>().devices[0]);

        // Find our podium
        podium = GameObject.Find("P" + playerNum + " Podium").GetComponent<PodiumControl>();

        // Find charToStageSelect
        charToStageSelect = GameObject.Find("CharStageSelect").GetComponent<CharToStageSelect>();

        // All set!
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the cursor
        // Don't go out of bounds
        if ((transform.position.x <= -19.75f && leftJoystick.x > 0) || (transform.position.x >= -13.25f && leftJoystick.x < 0))
        {
            leftJoystick = new Vector2(0, leftJoystick.y);
        }
        if ((transform.position.y >= 3.3f && leftJoystick.y > 0) || (transform.position.y <= -0.35f && leftJoystick.y < 0))
        {
            leftJoystick = new Vector2(leftJoystick.x, 0);
        }
        transform.Translate(leftJoystick.x * Time.deltaTime * trackingSpeed, leftJoystick.y * Time.deltaTime * trackingSpeed, 0);
    }

    // this is called when we go to the stage select screen to prevent the hand from going wild in the background
    public void Freeze()
    {
        ready = false;
    }

    private void OnMove(InputValue value)
    {
        if (ready)
        {
            leftJoystick = value.Get<Vector2>();
        }
    }

    // Place our token
    private void OnSelect()
    {
        if (ready)
        {
            bool done = false;

            // Do graphics raycast to see if we clicked a button
            pointEventData.position = Camera.main.WorldToScreenPoint(transform.position);
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointEventData, results);

            // Analyze the results
            if (results.Count > 1)
            {
                foreach (RaycastResult result in results)
                {
                    // We've got a button!
                    Debug.Log("Hit");
                    if (result.gameObject.CompareTag("CursorButton"))
                    {
                        result.gameObject.GetComponent<ICursorButtonable>().Click();
                        done = true;
                        break;
                    }
                }
            }

            if (hold && !done)
            {
                // Use FindWithTag here because it entirely depends on which group of character icons is currently active
                token.SetParent(GameObject.FindWithTag("CharacterIcons").transform);
                cursor.SetParent(transform);
                Debug.Log(token.parent);
                hold = false;
                tokCtrl.SetHeld(hold);
            }
        }
    }

    // Retrieve our token
    private void OnCancel()
    {
        if (ready)
        {
            if (!hold)
            {
                token.SetParent(transform);
                token.position = transform.position;
                cursor.SetParent(token);
                hold = true;
                tokCtrl.SetHeld(hold);
            }
        }
    }

    // Change our selected character's skin (currently only Calvin has another skin option)
    private void OnChangeSkin()
    {
        if(ready)
        {
            if(!hold)
            {
                tokCtrl.ChangeSkin();
            }
        }

    }

    // Start Button (normally "+" in Smash) is pressed to begin the game
    private void OnStart()
    {
        if (ready)
        {
            // Determine if all the players have selected a character
            if (playerCharList.AllSelected())
            {
                // transition to the stage select screen
                charToStageSelect.Transition(true);
            }
        }
    }

    private void OnToggleRotate()
    {
        if(ready)
        {
            podium.ToggleRotate();
        }
    }
}
