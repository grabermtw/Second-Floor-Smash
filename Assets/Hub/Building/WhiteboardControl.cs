using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardControl : MonoBehaviour
{
    public Sprite[] images;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if (images.Length > 0)
        {
            spriteRenderer.sprite = images[Random.Range(0, images.Length)];
        }
    }
}
