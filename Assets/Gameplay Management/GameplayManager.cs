﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement; // this is only needed when running from Unity editor

public class GameplayManager : MonoBehaviour
{

    private float upBoundary; // Go past this with a greater y velocity than maxUpVelocity and you die
    private float downBoundary; // Go past this and you die
    private float leftBoundary; // If |your x position| > this then you die
    private float rightBoundary;
    public CinemachineTargetGroup targetGroup;
    public GameObject intialPositions;
    public float maxUpVelocity;
    public GameObject[] playerTextObjects; // UI objects containing all the players' UI
    public TextMeshProUGUI[] playerDamageTexts; // GameObjects with the damage text TextMeshes on them
    public TextMeshProUGUI[] playerStockTexts; // TextMeshes with the remaining stock text
    public GameObject[] KOs; // Gameobjects containing the KO GameObjects (the fwooshes)
    private SmashSettings smashSettings;
    private List<GameObject> playerChoices;
    private List<InputDevice> playerDevices;
    private Rigidbody2D[] playerRbs;
    private int[] playerStocks;
    private GameObject sceneManage;

    [SerializeField]
    private GameObject[] testCharacters = default; // This will be used only for testing the stage so that you don't have to run it from the hub every time
    private bool test = false;

    void Awake()
    {
        // Find the scene manager if it's here
        sceneManage = GameObject.FindWithTag("SceneManager");
        try
        {
            smashSettings = GameObject.Find("SmashSettings").GetComponent<SmashSettings>();
        }
        catch
        {
            smashSettings = null;
        }

        // set boundaries based on collider
        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D boundaryCollider))
        {
            upBoundary = boundaryCollider.bounds.center.y + boundaryCollider.bounds.extents.y;
            downBoundary = boundaryCollider.bounds.center.y - boundaryCollider.bounds.extents.y;
            leftBoundary = boundaryCollider.bounds.center.x - boundaryCollider.bounds.extents.x;
            rightBoundary = boundaryCollider.bounds.center.x + boundaryCollider.bounds.extents.x;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // If we're actually playing the game
        if (smashSettings != null)
        {
            playerChoices = smashSettings.GetCharList();
            playerDevices = smashSettings.GetDeviceList();
            playerRbs = new Rigidbody2D[playerChoices.Count];
            playerStocks = new int[playerChoices.Count];

            Debug.Log("Bout to enter the foreach!");
            // Instantiate all the chosen characters
            // Also assign stocks
            for (int i = 0; i < playerChoices.Count; i++)
            {
                Spawn(i, true);
                playerStocks[i] = smashSettings.GetStock();
            }
        }
        else // SmashSettings isn't present so we must be just doing a test run
        {
            test = true;
            playerRbs = new Rigidbody2D[testCharacters.Length];
            playerStocks = new int[testCharacters.Length];
            for (int i = 0; i < testCharacters.Length; i++)
            {
                playerStocks[i] = -1;
                Spawn(i, true);
            }
        }
    }

    public void Spawn(int playerIndex, bool initial)
    {
        GameObject newPlayer;
        if (test) // If we're simply testing our stage
        {
            newPlayer = Instantiate(testCharacters[playerIndex]);
            
            // We only want to move the first player, so remove the player inputs from everyone else
            if (playerIndex != 0)
            {
                Destroy(newPlayer.GetComponent<PlayerInput>());
            }
            else
            {
                // enable WASD controls for player 1 in test mode only
                newPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("GameplayWASD");

            }
        }
        else // If we're actually playing the game
        {
            // Instantiate the players using PlayerInput so that the controllers are assigned correctly
            Debug.Log("Instantiating " + playerChoices[playerIndex].tag);
            Debug.Log("Device Number " + playerIndex + " out of " + playerDevices.Count);
            newPlayer = PlayerInput.Instantiate(playerChoices[playerIndex], -1, null, -1, playerDevices[playerIndex]).gameObject;
            Debug.Log("Instantiated " + playerChoices[playerIndex].tag);
        }
        // Get the player's rigidbody
        playerRbs[playerIndex] = newPlayer.GetComponent<Rigidbody2D>();

        // Set the character's layer. This will be how the character gets its player number as well.
        newPlayer.layer = 9 + playerIndex;

        if (initial)
        {
            // Move the player to the appropriate initial position
            newPlayer.transform.position = intialPositions.transform.Find("P" + (playerIndex + 1).ToString()).position;
        }
        else
        {
            newPlayer.transform.position = new Vector3(0, 5, 0);
        }

        // Add each player as a target for the camera
        AddPlayerToTargetGroup(newPlayer);
    }

    public void AddPlayerToTargetGroup(GameObject player)
    {
        // Add character as a target for the camera
        CinemachineTargetGroup.Target target;
        target.target = null;
        // Find the character's hips, because otherwise the camera will focus on their feet
        foreach (Transform tr in player.transform.GetComponentsInChildren<Transform>())
        {
            if (tr.gameObject.name == "mixamorig:Hips")
            {
                target.target = tr;
                break;
            }
        }
        target.weight = 1;
        target.radius = 1;
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target == null)
            {
                targetGroup.m_Targets.SetValue(target, i);
                break;
            }
        }
    }


    public TextMeshProUGUI GetDamageText(int playerNum)
    {

        playerTextObjects[playerNum - 1].SetActive(true);
        playerStockTexts[playerNum - 1].text = "Stock: " + playerStocks[playerNum - 1];
        return playerDamageTexts[playerNum - 1];
    }


    // Update is called once per frame
    // Here we'll keep track of the remaining living players and handle their deaths
    void Update()
    {
        for (int i = 0; i < playerRbs.Length; i++)
        {
            if (playerRbs[i] != null)
            {
                if (playerRbs[i].position.y < downBoundary)
                {
                    KO(i);
                }
                else if (playerRbs[i].position.x < leftBoundary)
                {
                    KO(i);
                }
                else if (playerRbs[i].position.x > rightBoundary)
                {
                    KO(i);
                }
                else if (playerRbs[i].position.y > upBoundary && playerRbs[i].velocity.y > maxUpVelocity)
                {
                    KO(i);
                }
            }
        }
    }

    // Handle KOs
    private void KO(int playerIndex)
    {
        // Use the player's KO position to instantiate the particle effect for the KO and orient it appropriately
        Vector3 pos = playerRbs[playerIndex].gameObject.transform.position;
        GameObject koParticles = Instantiate(KOs[playerIndex], pos, Quaternion.identity);
        koParticles.transform.LookAt(new Vector3(Random.Range(-2, 2), Random.Range(-0.5f, 4), 0));

        // Destroy the KO'd player
        Destroy(playerRbs[playerIndex].gameObject);

        // Subtrack from their remaining stock
        if (!test)
        {
            playerStocks[playerIndex] -= 1;
            playerStockTexts[playerIndex].text = "Stock: " + playerStocks[playerIndex];
        }
    }

    // This is an overload to be called externally when a player object has already been destroyed by other means.
    // Example: When Tim drinks someone in the Tim's Tastings stage.
    // Parameters:
    // playerIndex: index of the player being killed (0-3)
    // koInitialPos: starting point of the fwoosh.
    // koTargetPos: ending point of the fwoosh.
    // (optional) fwoosh: if true then do the fwoosh
    // (optional) condemned: GameObject to be destroyed at this time, if applicable. Exclude if not applicable.
    public void KO(int playerIndex, Vector3 koInitialPos, Vector3 koTargetPos, bool fwoosh = true, GameObject condemned = null)
    {
        // Destroy the condemned
        if (condemned != null)
        {
            Destroy(condemned);
        }

        // Do fwoosh if applicable
        if (fwoosh)
        {
            GameObject koParticles = Instantiate(KOs[playerIndex], koInitialPos, Quaternion.identity);
            koParticles.transform.LookAt(koTargetPos);
        }
        
        // Subtrack from their remaining stock
        if (!test)
        {
            playerStocks[playerIndex] -= 1;
            playerStockTexts[playerIndex].text = "Stock: " + playerStocks[playerIndex];
        }
    }


    public void FinishKO(int playerIndex)
    {
        // If we still have stock left, respawn
        if (playerStocks[playerIndex] != 0)
        {
            Spawn(playerIndex, false);
        }
        else // No stock so we dead
        {
            playerDamageTexts[playerIndex].text = "KO";
            smashSettings.AddToResults(playerIndex);

            List<int> results = smashSettings.GetResults();

            // Is the round over? Is everyone but the champion dead?
            if (results.Count >= playerChoices.Count - 1)
            {
                if (playerChoices.Count > 1)
                {
                    // We need to add the winning player's index to the end of the results
                    for (int i = 0; i < 3; i++)
                    {
                        if (playerChoices.Count >= 1 + i && !results.Contains(i))
                        {
                            smashSettings.AddToResults(i);
                        }
                    }
                }

                // Round is over. Time to go to the finish screen!
                if (sceneManage != null) // For development purposes so we don't have to start from the opening scene every time
                {
                    sceneManage.GetComponent<SceneControl>().LoadNextScene(2, false);
                }
                else
                {
                    SceneManager.LoadScene(2);
                }
            }
        }
    }

    // return reference to playerRbs
    public Rigidbody2D[] GetPlayerRbs()
    {
        return playerRbs;
    }

    public Transform ChooseRandomTarget()
    {
        // count number of players
        int numPlayers = 0;
        Transform[] players;
        Rigidbody2D[] rbs = GetPlayerRbs();
        for (int i = 0; i < rbs.Length; i++)
        {
            if (rbs[i] != null)
                numPlayers++;
        }
        // get transforms of players still alive
        players = new Transform[numPlayers];
        int j = 0;
        for (int i = 0; i < rbs.Length; i++)
        {
            if (rbs[i] != null)
                players[j++] = rbs[i].transform;
        }
        // return random target
        return players[Random.Range(0, numPlayers)];
    }
}
