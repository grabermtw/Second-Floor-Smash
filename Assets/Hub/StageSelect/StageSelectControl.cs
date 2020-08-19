using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectControl : MonoBehaviour
{
    private Vector2 leftJoystick; // holds the directions that the left joystick is pointing
    private float trackingSpeed = 3.5f; // how fast should the cursor move
    private Transform canvas;
    private PointerEventData pointEventData = new PointerEventData(null);
    private GraphicRaycaster gr;
    private ICursorButtonable currentButton;
    private bool ready = false;


    void Awake()
    {
        canvas = GameObject.Find("StageSelectCanvas").transform;
        transform.SetParent(canvas);
        transform.rotation = canvas.parent.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Prepare the graphics raycaster so we can click on things
        gr = canvas.GetComponent<GraphicRaycaster>();

        // Set an appropriate position and scale
        transform.localPosition = new Vector2(-245, -300);
        transform.localScale = new Vector3(1, 1, 1);

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
        if ((transform.position.y >= 3.25f && leftJoystick.y > 0) || (transform.position.y <= -0.35f && leftJoystick.y < 0))
        {
            leftJoystick = new Vector2(leftJoystick.x, 0);
        }
        transform.Translate(leftJoystick.x * Time.deltaTime * trackingSpeed, leftJoystick.y * Time.deltaTime * trackingSpeed, 0);
        
        // Do graphics raycast to see if we're hovering over a button
        pointEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointEventData, results);

        // Analyze the results
        if (results.Count > 1)
        {
            foreach (RaycastResult result in results)
            {
                // We've got a button!
                if (result.gameObject.CompareTag("CursorButton"))
                {
                    // New button so call HoverBegin() to begin hovering
                    if (currentButton == null)
                    {
                        currentButton = result.gameObject.GetComponent<ICursorButtonable>();
                        currentButton.HoverBegin();
                        break;
                    }
                    // Same button so call HoverStay() to continue hovering
                    else if (currentButton == result.gameObject.GetComponent<ICursorButtonable>())
                    {
                        currentButton.HoverStay();
                    }
                    else // Switch straight from one button to the next
                    {
                        currentButton.HoverExit();
                        currentButton = result.gameObject.GetComponent<ICursorButtonable>();
                        currentButton.HoverBegin();
                    }
                }
            }
        }
        else // hit nothing so get rid of the currentButton
        {
            if (currentButton != null)
            {
                currentButton.HoverExit();
                currentButton = null;
            }
        }
    }

    private void OnMove(InputValue value)
    {
        if (ready)
        {
            leftJoystick = value.Get<Vector2>();
        }
    }

    // When the user presses the select button
    void OnSelect()
    {
        if (ready && currentButton != null)
        {
            currentButton.Click();
        }
    }
}
