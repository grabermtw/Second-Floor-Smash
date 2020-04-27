using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitter : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Quit the game when escape is pressed
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
