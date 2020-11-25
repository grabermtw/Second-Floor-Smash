using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInstantiator : MonoBehaviour
{
    public CharacterList charList;
    public GameObject literallyEveryone;
    private List<GameObject[]> allChars;
    private int status = 0; // 0 is nobody in scene, 1 is one of everyone in scene, 2 is literally everyone in scene
    private GameObject[] currentChars;
    private GameObject currentLiterallyEveryone;

    // Start is called before the first frame update
    void Start()
    {
        allChars = charList.GetAllCharactersList();
        currentChars = new GameObject[allChars.Count];
        InstantiateNPCs();
    }

    public void InstantiateNPCs()
    {
        switch(status)
        {
            case 0:
                int i = 0;
                // Instantiate a random skin of each character
                foreach(GameObject[] character in allChars)
                {
                    currentChars[i] = Instantiate(character[(int) Mathf.Round(Random.Range(0, character.Length - 1))].GetComponent<CharacterController>().GetNonGameCharacter(), transform);
                    i++;
                }
                status = 1;
            break;
            case 1:
                // clear out the scene
                foreach(GameObject character in currentChars)
                {
                    Destroy(character);
                }
                currentChars = new GameObject[allChars.Count];
                currentLiterallyEveryone = Instantiate(literallyEveryone);
                status = 2;
            break;
            default:
                // remove everything
                Destroy(currentLiterallyEveryone);
                status = 0;
            break;
        }

    }
}
