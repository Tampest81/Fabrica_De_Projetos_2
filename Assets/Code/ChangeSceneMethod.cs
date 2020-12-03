using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneMethod : MonoBehaviour
{
    public string animation;
    public string nextScene;

    public void sceneChange()
    {
        SceneManager.LoadScene(nextScene);
    }
}
