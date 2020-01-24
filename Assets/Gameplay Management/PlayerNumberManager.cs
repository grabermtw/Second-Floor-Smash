using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNumberManager : MonoBehaviour
{
    private int playerNumber = 1;
    public GameObject[] playerTextObjects;
    public GameObject[] playerDamageTexts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPlayerNumber()
    {
        playerTextObjects[playerNumber - 1].SetActive(true);
        int result = playerNumber;
        playerNumber++;
        return result;
    }

    public TextMeshProUGUI GetDamageText(int playerNum)
    {
        return playerDamageTexts[playerNum - 1].GetComponent<TextMeshProUGUI>();
    }
}
