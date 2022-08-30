using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGroupButton : MonoBehaviour, ICursorButtonable
{
    public GameObject[] states;
    public GameObject nextState;

    public void HoverBegin()
    {

    }

    public void HoverStay()
    {

    }

    public void HoverExit()
    {

    }

    public void Click()
    {
        // Toggle whether the UMD people or the NY people are active
        foreach (GameObject state in states)
        {
            state.SetActive(false);
        }
        nextState.SetActive(true);
    }
}
