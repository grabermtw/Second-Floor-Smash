using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInterior : MonoBehaviour
{

    // This script handles the switch from 2D to 3D when Tim picks up his cup

    private List<GameObject> inCup;
    private List<int> playerIndices;

    void Awake()
    {
        inCup = new List<GameObject>();
        playerIndices = new List<int>();
    }

    // Add anyone who enters to the lists
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inCup.Contains(other.transform.parent.gameObject))
        {
            inCup.Add(other.transform.parent.gameObject);
        }
    }

    // Remove anyone who exits from the lists
    private void OnTriggerExit2D(Collider2D other)
    {
        inCup.Remove(other.transform.parent.gameObject);
    }

    public void PickUpCup()
    {
        for (int i = 0; i < inCup.Count; i++)
        {
            // Add the player's index to the list
            playerIndices.Add(inCup[i].GetComponent<CharacterController>().GetThisPlayerNumber() - 1);

            // Instantiate the 3D Non-Game version of the character in the spot that the 2d game character is in
            GameObject person3D = Instantiate(inCup[i].GetComponent<CharacterController>().GetNonGameCharacter(),
                                                inCup[i].transform.position, inCup[i].transform.rotation);
            // Remove the constraints that are on the Non-Game characters by default
            person3D.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            
            // Replace the 2D character in the list with the corresponding new 3D character
            GameObject person2D = inCup[i];
            inCup[i] = person3D;
            Destroy(person2D);
        }
    }

    // Clear both lists, used when Tim eats someone
    public void ResetLists()
    {
        inCup = new List<GameObject>();
        playerIndices = new List<int>();
    }

    public List<GameObject> GetInCupList()
    {
        return inCup;
    }

    public List<int> GetPlayerIndicesList()
    {
        return playerIndices;
    }


}
