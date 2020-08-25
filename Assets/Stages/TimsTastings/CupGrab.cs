using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CupGrab : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup targetGroup = default;
    [SerializeField]
    private GameplayManager gameplayManager = default;
    [SerializeField]
    private Transform grabbingHand = default;
    [SerializeField]
    private Transform cup = default;
    [SerializeField]
    private CupInterior cupInterior = default;
    [SerializeField]
    private Transform table = default;
    [SerializeField]
    private Transform KOOrigin = default; // Origin point for the fwoosh when someone dies being eaten by Tim
    [SerializeField]
    private Transform KOTarget = default; // Target point for the fwoosh when someone dies being eaten by Tim
    [SerializeField]
    private GameObject cupColliders2D = default;
    [SerializeField]
    private GameObject bottomSmack = default;
    private Vector3 initialCupPos;
    private Vector3 initialCupRot;
    private List<GameObject> cupContents;
    private List<int> cupPlayerIndices;

    void Awake()
    {
        initialCupPos = cup.position;
        initialCupRot = cup.localEulerAngles;
    }

    // Pick up the cup
    // Called by animation event when Tim grabs the cup
    private void PickUp()
    {
        cup.SetParent(grabbingHand);
        // Have CupInterior figure out who is in the cup and make them 3D
        cupInterior.PickUpCup();
        // Get CupInterior's report on who is in the cup
        cupContents = cupInterior.GetInCupList();
        cupPlayerIndices = cupInterior.GetPlayerIndicesList();

        // Set camera weight so that we can see Tim
        SetDrinkingCamTargetWeight(1);

        // Disable 2D cup colliders
        cupColliders2D.SetActive(false);
    }

    // Put down the cup
    // Called by animation event when cup reaches the table
    private void PutDown()
    {
        cup.SetParent(table);
        // Make sure it's put back in the right spot
        cup.position = initialCupPos;
        cup.localEulerAngles = initialCupRot;
        
        // Reset camera target weight
        SetDrinkingCamTargetWeight(0);

        // We shouldn't be able to get hurt by touching the cup now
        bottomSmack.SetActive(false);
    }

    // Kill anyone who is in the cup when Tim brings it to his lips
    private void SwallowPeople()
    {
        for (int i = 0; i < cupContents.Count; i++)
        {
            // Call gameplayManager's public KO method and have it do the dirty work for us.
            gameplayManager.KO(cupPlayerIndices[i], KOOrigin.position, KOTarget.position, true, cupContents[i]);
        }

        // Reset now that the cup's been cleared
        cupInterior.ResetLists();
    }

    // Called via animation event when Tim is putting the cup back.
    // This is called before the cup reaches the table.
    // Also enable CupBottomSmack so we can smack anyone who is under the cup when they're not supposed to be.
    private void Activate2DCupColliders()
    {
        cupColliders2D.SetActive(true);
        bottomSmack.SetActive(true);
    }

    
    private void SetDrinkingCamTargetWeight(float weight)
    {
        targetGroup.m_Targets[7].weight = weight;
    }
    
}
