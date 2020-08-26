using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtensionMethods;


public class StageMusic : MonoBehaviour
{

    [SerializeField]
    private AudioClip[] music = default;
    [SerializeField]
    private bool shuffle = default;
    private List<AudioClip> audioClipsQueue;
    private int audioIndex = 0;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioClipsQueue = music.ToList();
        if (shuffle)
        {
            audioClipsQueue.Shuffle();
        }
        else // When playing in order, start playing at random audio clip in the queue
        {
            audioIndex = Random.Range(0, audioClipsQueue.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
         if (audioClipsQueue.Count > 0)
        {
            bool playing = false;
            // Determine if we're playing anything at them moment.
            if(audioSource.isPlaying)
            {
                playing = true;
            }
            // Select and start new clip in queue
            if (!playing)
            {
                audioSource.clip = audioClipsQueue[audioIndex];            
                audioSource.timeSamples = 0;
                
                audioSource.Play();
                audioIndex += 1;
                // Wrap around
                if (audioIndex >= audioClipsQueue.Count)
                {
                    audioIndex = 0;
                    if (shuffle)
                    {
                        audioClipsQueue.Shuffle();
                        // Ensure we don't play the same clip twice in a row
                        if (audioSource.clip == audioClipsQueue[0])
                        {
                            audioIndex = 1;
                        }
                    }
                }
            }
        }
    }
}
