using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharList : MonoBehaviour
{
    public PlayerInputManager charSelectInput;
    public GameObject cursorPrefab;
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

    public void AddPlayer(InputDevice device)
    {
        // Prevent the device from needlessly being added to the device list again
        if (!playerDevices.Contains(device))
        {
            Debug.Log("Adding " + device);
            playerChoices.Add(null);
            playerDevices.Add(device);
        }
        charSelectInput.EnableJoining();
    }

    // This gets called when we re-enter the character select screen after previously exiting it
    // We need to do this so that the player cursors are mapped to the correct devices.
    public void RecreateExitedPlayers()
    {
        for (int i = 0; i < playerDevices.Count; i++)
        {
            Debug.Log("Instiantiating player number " + (i + 1));
            PlayerInput.Instantiate(cursorPrefab, -1, null, -1, playerDevices[i]);
        }
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