using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // this is only needed when running from Unity editor
using UnityEngine.InputSystem;
using TMPro;

public class GameFinishManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] winnerPlaces = default; // the positions that the winners should be assigned to
    [SerializeField]
    private GameObject[] numbers = default; // the big numbers that appear behind the winners
    [SerializeField]
    private Material[] materials = default; // the colors corresponding to each player number
    [SerializeField]
    private TextMeshProUGUI winnerText = default; // Text announcing the winner
    [SerializeField]
    private TextMeshProUGUI[] playerTexts = default; // Texts for places 1 thru 4

    private SmashSettings smashSettings;
    private GameObject sceneManage;

    private bool ready = false;



    void Awake()
    {
        sceneManage = GameObject.FindWithTag("SceneManager");
        smashSettings = GameObject.Find("SmashSettings").GetComponent<SmashSettings>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the results and the list of characters and player devices
        List<int> results = smashSettings.GetResults();
        List<GameObject> playerChoices = smashSettings.GetCharList();
        List<InputDevice> playerDevices = smashSettings.GetDeviceList();

        // Put the first place person at front of list, last place at back of list
        results.Reverse();

        // Assign the correct winner text
        winnerText.text = playerChoices[results[0]].tag + " Wins!";

        // Instantiate the winners in the proper order and positions
        for (int i = 0; i < results.Count; i++)
        {
            // Get our non-game character
            GameObject nonGameChar = playerChoices[results[i]].GetComponent<CharacterController>().GetNonGameCharacter();
            // Lower the positions appropriately
            winnerPlaces[i].position -= (4 - results.Count) * new Vector3(0, 2, 0);
            // Instantiate
            GameObject currentChar = Instantiate(nonGameChar, winnerPlaces[i]);
            // Trigger a random winning animation for the winner and random losing animations for the losers
            currentChar.GetComponent<Animator>().SetInteger("Won", i == 0 ? (int)Random.Range(1,14) : (int)Random.Range(15,29));
            // Give the placement numbers the appropriate colors based on the player numbers
            numbers[i].SetActive(true);
            numbers[i].GetComponent<Renderer>().material = materials[results[i]];
            // Activate and label the text objects appropriately
            playerTexts[i].gameObject.SetActive(true);
            playerTexts[i].text = "Player " + (results[i] + 1);
            playerTexts[i].gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>()[1].text = playerChoices[results[i]].tag;
        }
        ready = true;
    }

    // When the Start Button is pressed, we'll go back to the Hub.
    private void OnStart()
    {
        if (ready)
        {
            // Destroy the SmashSettings object so that we can start fresh when we return to the hub
            Destroy(GameObject.Find("SmashSettings"));
            if (sceneManage != null) // For development purposes so we don't have to start from the opening scene every time
            {
                sceneManage.GetComponent<SceneControl>().LoadNextScene(1, true);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
