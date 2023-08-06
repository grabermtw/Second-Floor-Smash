using System.Collections;
using UnityEngine;

public class JadePrism : MonoBehaviour
{
    public Transform prism;
    public Transform hiddenPos, exposedPos;
    public float transitionTime = 0.2f;
    
    private AudioSource audioSource;
    public float maxPitchVariance = 0.2f;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        prism.position = hiddenPos.position;
    }

    public void SetActive(bool val)
    {
        StartCoroutine(Transition(val));
    }

    private IEnumerator Transition(bool val)
    {
        float elapsedTime = 0f;

        Vector3 startPos = val ? hiddenPos.position : exposedPos.position;
        Vector3 endPos = val ? exposedPos.position : hiddenPos.position;

        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(1.0f - maxPitchVariance, 1.0f + maxPitchVariance);
            audioSource.Play();
        }

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            prism.position = Vector3.Lerp(startPos, endPos, elapsedTime / transitionTime);
            yield return null;
        }
    }
}