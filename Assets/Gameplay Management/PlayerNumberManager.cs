using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerNumberManager : MonoBehaviour
{
    private int playerNumber = 1;
    public GameObject[] playerTextObjects;
    public GameObject[] playerDamageTexts;
    public CinemachineTargetGroup targetGroup;
    private PlayerCharList playerCharList;
    private List<GameObject> playerChoices;
    private List<InputDevice> playerDevices;

    // Start is called before the first frame update
    void Start()
    {
        playerCharList = GameObject.Find("PlayerCharacterList").GetComponent<PlayerCharList>();
        playerChoices = playerCharList.GetCharList();
        playerDevices = playerCharList.GetDeviceList();

        Debug.Log("Bout to enter the foreach!");
        // Instantiate all the chosen characters
        int deviceNum = 0;
        foreach (GameObject choice in playerChoices)
        {
            Debug.Log("In the foreach!");

            Debug.Log("Instantiating " + choice.tag);
            Debug.Log("Device Number " + deviceNum + " out of " + playerDevices.Count);
            PlayerInput newPlayer = PlayerInput.Instantiate(choice, -1, null, -1, playerDevices[deviceNum]);
            Debug.Log("Instantiated " + choice.tag);
            deviceNum++;

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
