using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberCharacterSelect : MonoBehaviour
{
    private int playerNumber = 0;
    public GameObject[] playerCursors;

    public GameObject GetPlayerCursor()
    {
        playerNumber++;
        return playerCursors[playerNumber - 1];
    }

    public void ClearAll()
    {
        playerNumber = 0;
    }
}
