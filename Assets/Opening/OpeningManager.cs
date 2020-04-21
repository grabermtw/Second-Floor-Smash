using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    public bool UseRandomCharacters;
    public string placeholderCharacterTag;
    public CharacterList characterList;
    public Transform charPosParent;
    private Transform[] charPositions;
    private AudioManager audio;


    // Start is called before the first frame update
    void Awake()
    {
        if (UseRandomCharacters)
        {
            // Deactivate any placeholder characters that are currently in the scene
            Rigidbody[] placeholders = FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody rb in placeholders)
            {
                if (rb.gameObject.CompareTag(placeholderCharacterTag))
                {
                    rb.gameObject.SetActive(false);
                }
            }


            // Remember that charPositions[0] will be the parent's transform, which we'll want to ignore
            charPositions = charPosParent.GetComponentsInChildren<Transform>(false);

            List<GameObject[]> charList = characterList.GetAllCharactersList();

            System.Random rnd = new System.Random();

            // Start at i=1 to skip the parent transform
            for (int i = 1; i < charPositions.Length; i++)
            {
                int charIndex = rnd.Next(0, charList.Count);
                print("charIndex: " + charIndex);
                print("i = " + i);
                print("charList.Count: " + charList.Count);

                GameObject[] currChar = charList[charIndex];
                charList.RemoveAt(charIndex);

                Instantiate(currChar[(int)Random.Range(0, currChar.Length)], charPositions[i]);

            }
        }
    }

    void Start()
    {
        audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        audio.Play("Music");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Start Button (normally "+" in Smash) is pressed to begin the game
    private void OnStart()
    {
        SceneManager.LoadScene(1);
    }
}
