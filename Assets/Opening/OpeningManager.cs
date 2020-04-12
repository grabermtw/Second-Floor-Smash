using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{

    public CharacterList characterList;
    public Transform charPosParent;
    private Transform[] charPositions;


    // Start is called before the first frame update
    void Start()
    {
        // Remember that charPositions[0] will be the parent's transform, which we'll want to ignore
        charPositions = charPosParent.GetComponentsInChildren<Transform>(false);

        List<GameObject[]> charList = characterList.GetAllCharactersList();

        // Start at i=1 to skip the parent transform
        for (int i = 1; i < charPositions.Length; i++)
        {
            int charIndex = (int)Random.Range(0, charList.Count);
            GameObject[] currChar = charList[charIndex];
            charList.RemoveAt(charIndex);

            Instantiate(currChar[(int)Random.Range(0, currChar.Length)], charPositions[i]);
        }
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
