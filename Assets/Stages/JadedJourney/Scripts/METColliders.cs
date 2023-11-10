using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class METColliders : MonoBehaviour
{
    public GameObject collider2d;

    void OnTriggerEnter(Collider other)
    {
        collider2d.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        collider2d.SetActive(false);
    }
}
