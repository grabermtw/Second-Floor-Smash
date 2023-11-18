using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterData
{
    
    public string characterNameTag;
    public GameObject[] skins;
    public float characterSelectHeightOffset;
    public bool excludeFromOpening;
}

public class CharacterList : MonoBehaviour
{
    public CharacterData[] characterData;

    public bool DontDestroyOnLoadBool;

    private Dictionary<string, CharacterData> characters;

    void Awake()
    {
        if (DontDestroyOnLoadBool)
        {
            // We'd like to keep this consistent throughout the project
            DontDestroyOnLoad(this.gameObject);
        }

        // Make the above into a dictionary
        characters = characterData.ToDictionary(key => key.characterNameTag, CharacterData => CharacterData);
    }

    // Returns the array of skins for a specific character
    public GameObject[] GetCharacterArray(string characterName)
    {
        return characters[characterName].skins;
    }

    // Returns the height offset for the specific character (this is for the character selection screen)
    public float GetHeightOffset(string characterName)
    {
        try
        {
            return characters[characterName].characterSelectHeightOffset;
        }
        catch // Search all characters' tags in case a tag was specified that isn't the actual character's name
        {
            foreach (KeyValuePair<string, CharacterData> p in characters)
            {
                foreach(GameObject skin in p.Value.skins)
                {
                    if (skin.CompareTag(characterName))
                    {
                        return p.Value.characterSelectHeightOffset;
                    }
                }
            }
        }
        // default to 0 otherwise but this shouldn't happen
        Debug.LogWarning(characterName + "? Who the heck is this? Giving them a height offset of 0");
        return 0;
    }

    // Returns a list of all the arrays of skins for each character
    public List<GameObject[]> GetAllCharactersList(bool includeExclusions = false)
    {
        List<GameObject[]> charList = new List<GameObject[]>();
        foreach (CharacterData charData in characterData)
        {
            // TODO: handle exclusions
            if (includeExclusions || !charData.excludeFromOpening) 
            {
                charList.Add(charData.skins);
            }
        }
        return charList;
    }
}
