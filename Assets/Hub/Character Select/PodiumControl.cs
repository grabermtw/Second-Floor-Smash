using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumControl : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private GameObject currentCharacter;
    private bool rotate = false;
    

    // Start is called before the first frame update
    void Awake()
    {
        initialLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(0, 60 * Time.deltaTime, 0);
        }
    }

    public void ToggleRotate()
    {
        rotate = !rotate;
        if (rotate)
        {
            // Make sure that rotating doesn't make them fall off the poduim.
            // This is a lazy solution I know
            Rigidbody charRb = GetComponentInChildren<Rigidbody>();
            if (charRb != null)
            {
                charRb.isKinematic = true;
            }
            
        }
    }

    // Called by Token Control whenever a new character preview is instantiated
    public void AssignCurrentCharacter(GameObject character)
    {
        currentCharacter = character;
    }

    public void ResetRotation()
    {
        rotate = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Called by FPSToCharSelect, resets the position and clears out any leftover character previews.
    public void ResetPosition()
    {
        transform.localPosition = initialLocalPosition;
        Destroy(currentCharacter);
    }

    public Vector3 GetInitialLocalPosition()
    {
        return initialLocalPosition;
    }
}
