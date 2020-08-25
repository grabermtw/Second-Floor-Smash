using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRenderQueue : MonoBehaviour
{
    // This script's purpose is to make it so that people's hair doesn't
    // unnaturally shine through other faded/transparent materials.

    private Renderer cupRenderer;
    private int newQueue;

    void Awake()
    {
        cupRenderer = GetComponent<Renderer>();
        newQueue = cupRenderer.material.renderQueue + 1;
        cupRenderer.material.renderQueue = newQueue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
