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
    private Transform[] cups = default;
    [SerializeField]
    private Transform table = default;
    [SerializeField]
    private Transform KOOrigin = default; // Origin point for the fwoosh when someone dies being eaten by Tim
    [SerializeField]
    private Transform KOTarget = default; // Target point for the fwoosh when someone dies being eaten by Tim
    private GameObject[] cupColliders2D;
    private CupBottomSmack[] bottomSmack;
    private CupInterior[] cupInterior;
    private Animator animator;
    private Vector3[] initialCupPos;
    private Vector3[] initialCupRot;
    private List<GameObject> cupContents;
    private List<int> cupPlayerIndices;
    private int cupNum; // the cup that is being manipulated

    void Awake()
    {
        // Initialize arrays
        initialCupPos = new Vector3[cups.Length];
        initialCupRot = new Vector3[cups.Length];
        cupColliders2D = new GameObject[cups.Length];
        bottomSmack = new CupBottomSmack[cups.Length];
        cupInterior = new CupInterior[cups.Length];
        // Populate arrays with each cups info
        for (int i = 0; i < cups.Length; i++)
        {
            initialCupPos[i] = cups[i].position;
            initialCupRot[i] = cups[i].localEulerAngles;
            cupColliders2D[i] = cups[i].GetChild(0).gameObject;
            bottomSmack[i] = cups[i].GetComponentInChildren<CupBottomSmack>();
            // Disable at start
            bottomSmack[i].gameObject.SetActive(false);
            cupInterior[i] = cups[i].GetComponentInChildren<CupInterior>();
        }
        animator = GetComponent<Animator>();
        // Get a random cupNum to start with
        cupNum = Random.Range(0,3);
        // Set the animator to start
        animator.SetInteger("CupNum", cupNum + 1);
    }

    // Pick up the cup
    // Called by animation event when Tim grabs the cup
    private void PickUp()
    {
        cups[cupNum].SetParent(grabbingHand);
        // Have CupInterior figure out who is in the cup and make them 3D
        cupInterior[cupNum].PickUpCup();
        // Get CupInterior's report on who is in the cup
        cupContents = cupInterior[cupNum].GetInCupList();
        cupPlayerIndices = cupInterior[cupNum].GetPlayerIndicesList();

        // If someone is being swallowed, set camera weight so that we can see Tim
        if (cupContents.Count > 0)
        {
            SetDrinkingCamTargetWeight(1);
        }

        // Disable 2D cup colliders
        cupColliders2D[cupNum].SetActive(false);
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
        cupInterior[cupNum].ResetLists();
    }

    // Called via animation event when Tim is putting the cup back.
    // This is called before the cup reaches the table.
    // Also enable CupBottomSmack so we can smack anyone who is under the cup when they're not supposed to be.
    private void Activate2DCupColliders()
    {
        cupColliders2D[cupNum].SetActive(true);
        bottomSmack[cupNum].gameObject.SetActive(true);
    }

    // Put down the cup
    // Called by animation event when cup reaches the table
    private void PutDown()
    {
        cups[cupNum].SetParent(table);
        // Make sure it's put back in the right spot
        cups[cupNum].position = initialCupPos[cupNum];
        cups[cupNum].localEulerAngles = initialCupRot[cupNum];
        
        // Reset camera target weight
        SetDrinkingCamTargetWeight(0);

        // We shouldn't be able to get hurt by touching the cup now
        bottomSmack[cupNum].gameObject.SetActive(false);

        // This is the last action that takes place after the cup is picked up,
        // so we'll decide on our next cup to pick up and let the animator know.
        cupNum = Random.Range(0,4);
        if (cupNum == 4)
            cupNum = 3;
        animator.SetInteger("CupNum", cupNum + 1);
    }
    
    private void SetDrinkingCamTargetWeight(float weight)
    {
        // The target is in index 7
        targetGroup.m_Targets[7].weight = weight;
    }
    
}
