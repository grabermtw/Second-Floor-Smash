﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGroupButton : MonoBehaviour, ICursorButtonable
{
    public GameObject md;
    public GameObject ny;

    public void Hover()
    {

    }

    public void Click()
    {
        // Toggle whether the UMD people or the NY people are active
        md.SetActive(!md.activeSelf);
        ny.SetActive(!ny.activeSelf);
    }
}
