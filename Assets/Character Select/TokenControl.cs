using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenControl : MonoBehaviour
{
    private GraphicRaycaster gr;
    private PointerEventData pointEventData = new PointerEventData(null);

    // Start is called before the first frame update
    void Start()
    {
        // Get ready for some raycasting!
        GameObject canvas = GameObject.Find("Canvas");
        gr = canvas.GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do the raycast
        pointEventData.position = transform.position;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointEventData, results);

        // Analyze the results
        if(results.Count > 1){
            foreach(RaycastResult result in results){
                // We've got an icon!
                if(result.gameObject.layer == 16)
                {
                    // Print the name of the character
                    Debug.Log(result.gameObject.tag);
                    break;
                }
            }
        }
        
    }

}
