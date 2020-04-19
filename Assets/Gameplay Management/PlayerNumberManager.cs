using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerNumberManager : MonoBehaviour
{
    public float maxUpVelocity; //
    public float upBoundary; // Go past this with a greater y velocity than maxUpVelocity and you die
    public float downBoundary; // Go past this and you die
    public float sideBoundary; // If |your x position| > this then you die
    public GameObject[] playerTextObjects;
    public GameObject[] playerDamageTexts;
    public GameObject[] KOs;
    public CinemachineTargetGroup targetGroup;
    public GameObject intialPositions;
    private PlayerCharList playerCharList;
    private List<GameObject> playerChoices;
    private List<InputDevice> playerDevices;
    private Rigidbody2D[] playerRbs;

    // Start is called before the first frame update
    void Start()
    {
        playerCharList = GameObject.Find("PlayerCharacterList").GetComponent<PlayerCharList>();
        playerChoices = playerCharList.GetCharList();
        playerDevices = playerCharList.GetDeviceList();
        playerRbs = new Rigidbody2D[playerChoices.Count];

        Debug.Log("Bout to enter the foreach!");
        // Instantiate all the chosen characters
        for (int i = 0; i < playerChoices.Count; i++)
        {
            Spawn(i, true);
        }
    }

    public void Spawn(int playerIndex, bool initial)
    {
        // Instantiate the players using PlayerInput so that the controllers are assigned correctly
        Debug.Log("Instantiating " + playerChoices[playerIndex].tag);
        Debug.Log("Device Number " + playerIndex + " out of " + playerDevices.Count);
        PlayerInput newPlayer = PlayerInput.Instantiate(playerChoices[playerIndex], -1, null, -1, playerDevices[playerIndex]);
        Debug.Log("Instantiated " + playerChoices[playerIndex].tag);

        // Get the player's rigidbody
        playerRbs[playerIndex] = newPlayer.gameObject.GetComponent<Rigidbody2D>();

        // Set the character's layer. This will be how the character gets it's player number as well.
        newPlayer.gameObject.layer = 9 + playerIndex;

        if (initial)
        {
            // Move the player to the appropriate initial position
            newPlayer.transform.position = intialPositions.transform.Find("P" + (playerIndex + 1).ToString()).position;
        }
        else
        {
            newPlayer.transform.position = new Vector3(0, 5, 0);
        }

        // Add each character as a target for the camera
        CinemachineTargetGroup.Target target;
        target.target = null;
        // Find the character's hips, because otherwise the camera will focus on their feet
        foreach (Transform tr in newPlayer.gameObject.transform.GetComponentsInChildren<Transform>())
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
        return playerDamageTexts[playerNum - 1].GetComponent<TextMeshProUGUI>();
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
                    KO(i, 0);
                }
                else if (Mathf.Abs(playerRbs[i].position.x) > sideBoundary)
                {
                    KO(i, 2);
                }
                else if (playerRbs[i].position.y > upBoundary && playerRbs[i].velocity.y > maxUpVelocity)
                {
                    KO(i, 1);
                }
            }
        }
    }

    // Handle KOs
    // KOSide referrs to the direction they were KO'd in
    // KOSide=0: Down
    // KOSide=1: Up
    // KOSide=2: left or right
    private void KO(int playerIndex, int KOSide)
    {
        // Use the player's KO position to instantiate the particle effect for the KO and orient it appropriately
        Vector3 pos = playerRbs[playerIndex].gameObject.transform.position;
        GameObject koParticles = Instantiate(KOs[playerIndex], pos, Quaternion.identity);
        koParticles.transform.LookAt(new Vector3(Random.Range(-2, 2), Random.Range(-0.5f, 4), 0));

        // Destroy the KO'd player
        Destroy(playerRbs[playerIndex].gameObject);
    }
}
