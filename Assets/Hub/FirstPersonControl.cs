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
        rb.MovePosition(rb.position + movement);
        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, lookHorizontal, 0)));
        fpCam.Rotate(lookVertical, 0, 0);
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

}
