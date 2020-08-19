using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StockCounter : MonoBehaviour, ICursorButtonable
{
    public bool increaseCount;
    private SmashSettings smashSettings;

    // Start is called before the first frame update
    void Start()
    {
        smashSettings = GameObject.Find("SmashSettings").GetComponent<SmashSettings>();
    }
    
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
        if (increaseCount)
        {
            if (smashSettings.GetStock() < 99)
            {
                // Increase our stock
                smashSettings.IncrementStock(1);
            }
        }
        else if (smashSettings.GetStock() > 1)
        {
            // Decrease our stock if we wouldn't be going negative
            smashSettings.IncrementStock(-1);
        }
    }
}
