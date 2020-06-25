using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumControl : MonoBehaviour
{

    private bool rotate = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(0, 1, 0);
        }
    }

    public void ToggleRotate()
    {
        rotate = !rotate;
        if (rotate)
        {
            // Make sure that rotating doesn't make them fall off the poduim.
            // This is a lazy solution I know
            Rigidbody2D charRb = GetComponentInChildren<Rigidbody2D>();
            if (charRb != null)
            {
                charRb.isKinematic = true;
            }
        }
    }

    public void ResetRotation()
    {
        rotate = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
