using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharList : MonoBehaviour
{
    List<GameObject> playerChoices;
    List<InputDevice> playerDevices;
    bool finishedSelect = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerChoices = new List<GameObject>();
        playerDevices = new List<InputDevice>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayer(InputDevice device)
    {
        playerChoices.Add(null);
        playerDevices.Add(device);
    }

    // Choose characters
    public void ChooseCharacter(GameObject character, int player)
    {
        playerChoices[player - 1] = character;
        if (character != null)
        {
            Debug.Log(playerChoices[player - 1]);
        }

        // Determine if all the players have selected a character yet

        for (int i = 0; i < playerChoices.Count; i++)
        {
            if (playerChoices[i] == null)
            {
                finishedSelect = false;
                return;
            }
        }
        finishedSelect = true;
    }

    // Lets us know if we're all selected so we can begin playing the game
    public bool AllSelected()
    {
        return finishedSelect;
    }

    public List<GameObject> GetCharList()
    {
        return playerChoices;
    }

    public List<InputDevice> GetDeviceList()
    {
        return playerDevices;
    }
}