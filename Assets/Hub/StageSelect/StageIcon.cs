using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageIcon : MonoBehaviour, ICursorButtonable
{
    public string stageName;
    public int sceneIndex;
    private GameObject sceneManage;

    // Start is called before the first frame update
    void Start()
    {
        sceneManage = GameObject.FindWithTag("SceneManager");
    }

    public void Hover()
    {

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
