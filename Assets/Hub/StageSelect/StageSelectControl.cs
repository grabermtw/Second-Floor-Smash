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
    private bool ready = false;


    void Awake()
    {
        canvas = GameObject.Find("StageSelectCanvas").transform;
        transform.parent = canvas;
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
        transform.Translate(leftJoystick.x * Time.deltaTime * trackingSpeed, leftJoystick.y * Time.deltaTime * trackingSpeed, 0);
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
        if (ready)
        {
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
                        break;
                    }
                }
            }
        }
    }
}
