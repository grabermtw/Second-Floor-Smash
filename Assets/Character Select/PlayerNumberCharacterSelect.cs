using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberCharacterSelect : MonoBehaviour
{
    private int playerNumber = 0;
    public GameObject[] playerCursors;
    public PlayerCharList playerCharList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPlayerCursor()
    {
        playerNumber++;
        return playerCursors[playerNumber - 1];
    }
}
