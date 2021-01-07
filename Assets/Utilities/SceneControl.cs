using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneControl : MonoBehaviour
{
    public Animator fader;
    public Image darkness;
    public TextMeshProUGUI loadingQualityText;
    private bool fadeInNextScene = true;
    private int previousScene;
    private int currentQualityLevel;
    private string[] qualityLevels = {"Very Low", "Low", "Medium", "High", "Very High", "Ultra"};
    private int displayTime;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentQualityLevel = QualitySettings.GetQualityLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // Cycle through quality levels
        // Change this eventually hopefully
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Q))
        {
            currentQualityLevel = (currentQualityLevel + 1) % qualityLevels.Length;
            QualitySettings.SetQualityLevel(currentQualityLevel);
            StartCoroutine(DisplayQualityText());
        }
    }

    // Temporarily display the quality setting.
    private IEnumerator DisplayQualityText()
    {
        loadingQualityText.text = "Quality: " + qualityLevels[currentQualityLevel];
        // if we're already displaying the quality setting, add some additional time
        if (displayTime > 0 && displayTime < 4)
        {
            displayTime = 3;
            yield break;
        }
        displayTime = 3;
        while (displayTime > 0)
        {
            yield return new WaitForSeconds(1);
            displayTime--;
            yield return null;
        }
        loadingQualityText.text = "";
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
        loadingQualityText.text = "Loading...";
        previousScene = SceneManager.GetActiveScene().buildIndex;
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
        loadingQualityText.text = "";
        if (fadeInNextScene)
        {
            fader.SetTrigger("FadeIn");
        }
    }

    public int GetPreviousSceneNumber()
    {
        return previousScene;
    }
}
