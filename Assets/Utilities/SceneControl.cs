using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public Animator fader;
    public Image darkness;
    private bool fadeInNextScene = true;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadNextScene(int sceneNum, bool fadeIn)
    {
        fadeInNextScene = fadeIn;
        // Make sure the screen is dark
        if(fadeInNextScene)
        {
            darkness.color = new Color32(0,0,0,255);
        }
        SceneManager.LoadScene(sceneNum);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(fadeInNextScene)
        {
            fader.SetTrigger("FadeIn");
        }
    }
}
