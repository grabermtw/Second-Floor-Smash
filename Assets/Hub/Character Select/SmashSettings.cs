using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SmashSettings : MonoBehaviour
{
    private const int DEFAULT_STOCK = 3;

    public PlayerInputManager charSelectInput;
    public GameObject cursorPrefab;
    public TextMeshProUGUI counterText;
    [SerializeField]
    private GameObject pressStart = default; // GameObject that will be activated whenever all players have selected a character
    private int stock;
    List<GameObject> playerChoices; // List of characters the players will be using
    List<InputDevice> playerDevices; // List of devices the players will be using
    List<int> results; // List of the players in order of who won to who lost for use in GameFinish
    bool finishedSelect = false;



    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerChoices = new List<GameObject>();
        playerDevices = new List<InputDevice>();
        results = new List<int>();
        ResetStock();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // ------------- CHARACTER SELECT SECTION -------------
    // These are called when we're in the Hub at the character select screen.

    public void AddPlayer(InputDevice device)
    {
        // We can't continue until the new person has selected a character
        finishedSelect = false;
        pressStart.SetActive(false);
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
                pressStart.SetActive(false);
                return;
            }
        }
        finishedSelect = true;
        pressStart.SetActive(true);
    }

    // Lets us know if we're all selected so we can begin playing the game
    public bool AllSelected()
    {
        return finishedSelect;
    }

    // Stock Stuff:
    public void IncrementStock(int increment)
    {
        stock += increment;
        counterText.text = stock.ToString();
    }
    
    public void ResetStock()
    {
        stock = DEFAULT_STOCK;
        counterText.text = stock.ToString();
    }


    // ------------- GAMEPLAY SECTION -------------
    // These are called when setting up a stage to smash on

    public int GetStock()
    {
        return stock;
    }

    public List<GameObject> GetCharList()
    {
        return playerChoices;
    }

    public List<InputDevice> GetDeviceList()
    {
        return playerDevices;
    }


    // ------------- POST-GAMEPLAY SECTION -------------
    // These are called when the game is over and we're going to the GameFinish scene

    // Add a fallen player to the results list
    public void AddToResults(int playerIndex)
    {
        results.Add(playerIndex);
    }

    // Return the results list
    public List<int> GetResults()
    {
        return results;
    }
}