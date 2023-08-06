using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JadedJourneyStageControl : MonoBehaviour
{

    public float minFlatTime = 30f, maxFlatTime = 90f;
    public float minPrismTime = 10f, maxPrismTime = 45f;
    private float transitionTimer;

    public float prismTransitionTime = 0.2f;
    private bool arePrismsActive = false;
    private bool arePrismsTransitioning = false;
    private JadePrism[] prisms;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        transitionTimer = Random.Range(minFlatTime, maxFlatTime);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        prisms = FindObjectsOfType<JadePrism>();
        foreach (JadePrism prism in prisms)
        {
            prism.transitionTime = prismTransitionTime;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        transitionTimer -= Time.deltaTime;

        if (transitionTimer <= 0 && !arePrismsTransitioning)
        {
            StartCoroutine(TransitionPrisms());
        }
    }

    private IEnumerator TransitionPrisms()
    {
        arePrismsActive = !arePrismsActive;
        arePrismsTransitioning = true;

        foreach (JadePrism prism in prisms)
        {
            prism.SetActive(arePrismsActive);
            yield return new WaitForSeconds(prismTransitionTime);
        }
        if (arePrismsActive)
        {
            transitionTimer = Random.Range(minPrismTime, minFlatTime);
        }
        else
        {
            transitionTimer = Random.Range(minFlatTime, maxFlatTime);
        }
        arePrismsTransitioning = false;
    }
}

    