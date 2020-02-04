using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterSelectControl : MonoBehaviour
{
    Vector2 leftJoystick; // holds the directions that the left joystick is pointing
    float trackingSpeed = 500f; // how fast should the cursor move
    Transform token; // The token we'll use to select our character
    Transform cursor; // Our cursor
    Transform canvas;
    bool ready = false; // Make sure everything's set before taking input
    PlayerNumberCharacterSelect playerManager;
    PlayerCharList playerCharList;
    TokenControl tokCtrl;
    bool hold = true;


    void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        transform.parent = canvas;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the appropriate cursor and token
        playerManager = GameObject.Find("PlayerInputManager").GetComponent<PlayerNumberCharacterSelect>();
        token = Instantiate(playerManager.GetPlayerCursor(), transform).transform;
        cursor = token.GetComponentsInChildren<Transform>()[1];
        tokCtrl = token.gameObject.GetComponent<TokenControl>();

        // Find the player character list for checking whether we can begin the game in OnStart
        playerCharList = GameObject.Find("PlayerCharacterList").GetComponent<PlayerCharList>();

        playerCharList.AddPlayer(GetComponent<PlayerInput>().devices[0]);

        // All set!
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the cursor
        transform.Translate(leftJoystick.x * Time.deltaTime * trackingSpeed, leftJoystick.y * Time.deltaTime * trackingSpeed, 0);
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
            if (hold)
            {
                token.SetParent(canvas);
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

    // Start Button (normally "+" in Smash) is pressed to begin the game
    private void OnStart()
    {
        if (ready)
        {
            // Determine if all the players have selected a character
            if (playerCharList.AllSelected())
            {
                SceneManager.LoadScene(1);
            }
        }
    }

}
