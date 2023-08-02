using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInstantiator : MonoBehaviour
{
    public enum Mode
    {
        Nobody,
        Everybody,
        LiterallyEverybody,
        RulesBased
    }

    public CharacterList charList;
    public GameObject literallyEveryone;
    private List<GameObject[]> allChars;
    private Mode status = Mode.RulesBased; // 0 is nobody in scene, 1 is one of everyone in scene, 2 is literally everyone in scene
    private GameObject[] currentChars;
    private GameObject currentLiterallyEveryone;

    // Start is called before the first frame update
    void Start()
    {
        allChars = charList.GetAllCharactersList(false);
        currentChars = new GameObject[allChars.Count];
        InstantiateNPCs();
    }

    public void InstantiateNPCs()
    {
        status = (Mode)(((int)status + 1 ) % System.Enum.GetValues(typeof(Mode)).Length);

        // always destroy all existing characters
        foreach(GameObject character in currentChars)
        {
            Destroy(character);
        }

        switch(status)
        {
            case Mode.Nobody:
                // nothing to do, we dont want to spawn anything
            break;
            case Mode.Everybody:
                int i = 0;
                // Instantiate a random skin of each character
                foreach(GameObject[] character in allChars)
                {
                    if (character.Length > 0)
                    {
                        currentChars[i] = Instantiate(character[(int) Mathf.Round(Random.Range(0, character.Length - 1))].GetComponent<CharacterController>().GetNonGameCharacter(), transform);
                        i++;
                    }
                }
            break;
            case Mode.LiterallyEverybody:
            break; //debug
                currentChars = new GameObject[allChars.Count];
                currentLiterallyEveryone = Instantiate(literallyEveryone);
                status = Mode.LiterallyEverybody;
            break;
            case Mode.RulesBased:
                // now spawn characters based on predefined spawn points

            break;
            default:
                // remove everything
                Destroy(currentLiterallyEveryone);
                status = 0;
            break;
        }

    }
}
