using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectControl : MonoBehaviour
{
    Vector2 leftJoystick; // holds the directions that the left joystick is pointing
    float trackingSpeed = 500f; // how fast should the cursor move
    Transform token; // The token we'll use to select our character
    Transform cursor; // Our cursor
    Transform canvas;
    bool ready = false; // Make sure everything's set before taking input
    PlayerNumberCharacterSelect playerManager;
    

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

        Debug.Log(token.gameObject.name);
        Debug.Log(cursor.gameObject.name);

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
            token.SetParent(canvas);
            cursor.SetParent(transform);
            Debug.Log(token.parent);
        }
    }

    // Retrieve our token
    private void OnCancel()
    {
        if (ready)
        {
            token.SetParent(transform);
            token.position = transform.position;
            cursor.SetParent(token);
        }
    }

}
