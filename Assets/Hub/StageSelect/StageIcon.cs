using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageIcon : MonoBehaviour, ICursorButtonable
{
    public string stageName;
    public int sceneIndex;
    [SerializeField]
    private TextMeshProUGUI stageText = default;
    private GameObject sceneManage;

    // Start is called before the first frame update
    void Start()
    {
        sceneManage = GameObject.FindWithTag("SceneManager");
    }

    public void HoverBegin()
    {
        stageText.enabled = true;
        stageText.text = stageName;
    }

    public void HoverStay()
    {

    }

    public void HoverExit()
    {
        stageText.enabled = false;
    }

    public void Click()
    {
        // For development purposes so we don't have to start from the opening scene every time
        if (sceneManage != null)
        {
            sceneManage.GetComponent<SceneControl>().LoadNextScene(sceneIndex, false);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
