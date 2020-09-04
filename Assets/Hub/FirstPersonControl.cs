using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FirstPersonControl : MonoBehaviour
{
    public float moveSpeed;
    public float lookSensitivity;
    public Text interactMessage;
    private Vector3 movement;
    private float lookHorizontal;
    private float lookVertical;
    private Rigidbody rb;
    private Transform fpCam;
    private HubInteractor currentInteract;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fpCam = transform.GetChild(0);

        // Determine where in Prince Frederick to start
        GameObject sceneManage = GameObject.FindWithTag("SceneManager");
        int prevScene = 0;
        if(sceneManage != null)
        {
            prevScene = sceneManage.GetComponent<SceneControl>().GetPreviousSceneNumber();
        }
        // If we didn't just come from the opening scene, then put us near the character select.
        if(prevScene != 0)
        {
            transform.position = new Vector3(-16.91f,1.173f,-26.89f);
        }
    }

    // When the left joystick is moved
    void OnMove(InputValue value)
    {
        Vector2 stick = value.Get<Vector2>();
        movement = transform.TransformDirection(new Vector3(stick.x * moveSpeed, 0, stick.y * moveSpeed));
    }

    // When the right joystick is moved
    void OnLook(InputValue value)
    {
        Vector2 stick = value.Get<Vector2>();
        lookHorizontal = stick.x * lookSensitivity;
        lookVertical = - stick.y * lookSensitivity;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, lookHorizontal * Time.fixedDeltaTime, 0)));
        fpCam.Rotate(lookVertical * Time.fixedDeltaTime, 0, 0);
        rb.velocity = new Vector3(0,0,0);
    }

    void OnAction()
    {
        if(currentInteract != null)
        {
            currentInteract.Interact();
        }
    }

    // Called by the interactable that we are within range of
    public void SetInteract(HubInteractor interactable, string message)
    {
        currentInteract = interactable;
        interactMessage.enabled = true;
        interactMessage.text = message;
    }

    // Called when we're no longer in range of interactable
    public void UnsetInteract()
    {
        currentInteract = null;
        interactMessage.enabled = false;
        
    }

    // ----------- Keyboard Actions (for when you don't have a gamepad handy) ------------
    void OnMoveLeftKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        movement = transform.TransformDirection(new Vector3(- inputVal * moveSpeed, 0, 0));
    }

    void OnMoveRightKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        movement = transform.TransformDirection(new Vector3(inputVal * moveSpeed, 0, 0));
    }

    void OnMoveForwardKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        movement = transform.TransformDirection(new Vector3(0, 0, inputVal * moveSpeed));
    }

    void OnMoveBackwardKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        movement = transform.TransformDirection(new Vector3(0, 0, - inputVal * moveSpeed));
    }

    void OnLookUpKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        lookVertical = - inputVal * lookSensitivity;
    }

    void OnLookDownKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        lookVertical = inputVal * lookSensitivity;
    }

    void OnLookLeftKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        lookHorizontal = - inputVal * lookSensitivity;
    }

    void OnLookRightKeyboard(InputValue value)
    {
        float inputVal = value.Get<float>();
        lookHorizontal = inputVal * lookSensitivity;
    }
}
