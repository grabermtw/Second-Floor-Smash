using System.Collections;
using UnityEngine;
using Cinemachine;

public class BigRobo : MonoBehaviour
{

    [SerializeField]
    private CinemachineTargetGroup targetGroup = default;
    [SerializeField]
    private Transform activePos, inactivePos;
    [SerializeField]
    private float transitionTime, minDelay, maxDelay;

    private float timeUntilTransition;
    private bool isActive;
    
    private void Start()
    {
        isActive = false;
        transform.position = inactivePos.position;
        targetGroup.m_Targets[7].weight = 0f;
        timeUntilTransition = Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        timeUntilTransition -= Time.deltaTime;
        if (timeUntilTransition <= 0)
        {
            timeUntilTransition = Random.Range(minDelay, maxDelay);
            isActive = !isActive;
            StartCoroutine(SetActive(isActive));
        }
    }

    private IEnumerator SetActive(bool active)
    {
        
        // The target is in index 7
        targetGroup.m_Targets[7].weight = active ? 0.4f : 0f;
        float elapsedTime = 0;
        Vector3 start = active ? inactivePos.position : activePos.position;
        Vector3 end = active ? activePos.position : inactivePos.position;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsedTime / transitionTime);
            yield return null;
        }
        
    }

}