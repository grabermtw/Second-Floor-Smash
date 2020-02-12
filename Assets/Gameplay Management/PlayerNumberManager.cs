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

        Debug.Log("Bout to enter the foreach!");
        // Instantiate all the chosen characters
        int deviceNum = 0;
        foreach (string choice in playerChoices)
        {
            Debug.Log("In the foreach!");
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].tag == choice)
                {
                    Debug.Log("Instantiating " + characters[i].tag);
                    Debug.Log("Device Number " + deviceNum + " out of " + playerDevices.Count);
                    PlayerInput.Instantiate(characters[i], i, null, -1, playerDevices[deviceNum]);
                    Debug.Log("Instantiated " + characters[i].tag);
                    deviceNum++;
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
