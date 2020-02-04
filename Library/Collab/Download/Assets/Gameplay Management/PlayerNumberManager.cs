using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerNumberManager : MonoBehaviour
{
    private int playerNumber = 1;
    public GameObject[] playerTextObjects;
    public GameObject[] playerDamageTexts;
    public GameObject[] characters;
    PlayerCharList playerCharList;
    List<string> playerChoices;
    List<InputDevice> playerDevices;

    // Start is called before the first frame update
    void Start()
    {
        playerCharList = GameObject.Find("PlayerCharacterList").GetComponent<PlayerCharList>();
        playerChoices = playerCharList.GetCharList();
        playerDevices = playerCharList.GetDeviceList();

        // Instantiate all the chosen characters
        foreach (string choice in playerChoices)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].tag == choice)
                {
                    PlayerInput.Instantiate(characters[i], i, null, -1, playerDevices[i]);
                }
            }
        }

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
