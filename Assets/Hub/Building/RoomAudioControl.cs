using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;

public class RoomAudioControl : MonoBehaviour
{
    [System.Serializable]
    public class RoomAudioClip
    {
        public AudioClip audioClip;
        public int audioSourceNumber;
    }

    public bool shuffle;
    public RoomAudioClip[] audioClips;
    public AudioSource[] audioSources;
    private List<RoomAudioClip> audioClipsQueue;
    private int audioIndex = 0;
    private bool firstPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        audioClipsQueue = audioClips.ToList();
        if (shuffle)
        {
            audioClipsQueue.Shuffle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioClipsQueue.Count > 0)
        {
            bool playing = false;
            // Determine if we're playing anything at them moment.
            foreach (AudioSource audio in audioSources)
            {
                if (audio.isPlaying)
                {
                    playing = true;
                }
            }
            if (!playing)
            {
                AudioSource currentAudio = audioSources[audioClipsQueue[audioIndex].audioSourceNumber];
                currentAudio.clip = audioClipsQueue[audioIndex].audioClip;
                // If this is the first time an audio clip is being played, then start at random time
                if (firstPlay)
                {
                    int startTime = Random.Range(0, currentAudio.clip.samples);
                    currentAudio.timeSamples = startTime;
                    firstPlay = false;
                }
                currentAudio.Play();
                audioIndex += 1;
                if (audioIndex >= audioClipsQueue.Count)
                {
                    audioClipsQueue.Shuffle();
                    audioIndex = 0;
                    // Ensure we don't play the same clip twice in a row
                    if (currentAudio.clip == audioClipsQueue[0].audioClip)
                    {
                        audioIndex = 1;
                    }
                }
            }
        }
    }

}
