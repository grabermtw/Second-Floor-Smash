using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageControl : MonoBehaviour
{
    public float initialDamage;
    private float damage;
    private GameplayManager playerManager;
    private int playerNumber;
    private TextMeshProUGUI damageDisplay;

    // Start is called before the first frame update
    void Start()
    {
        damage = initialDamage;
    }

    public void AssignPlayerNumber(int playerNum)
    {
        // Get the player number
        playerNumber = playerNum;
        playerManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        // Display the damage reading for this player
        damageDisplay = playerManager.GetDamageText(playerNumber);
        Debug.Log(damageDisplay);
        damageDisplay.text = initialDamage.ToString("F") + "%";
        // Display the character's name
        damageDisplay.gameObject.transform.parent.gameObject.GetComponent<TextMeshProUGUI>().text = "Player " + playerNumber + "\n" + gameObject.tag;
    }

    // Literally just returns the current damage
    public float GetDamage()
    {
        return damage;
    }

    public void UpdateDamage(float newDamage, bool add=true)
    {
        if(add)
        {
            damage += newDamage;
        }
        else
        {
            damage -= newDamage;
        }

        damageDisplay.text = damage.ToString("F") + "%";
    }


}
