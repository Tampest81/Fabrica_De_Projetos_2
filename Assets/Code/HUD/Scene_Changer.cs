using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Changer : MonoBehaviour
{
    public string Scene1;
    public string Scene2;
    public string Scene3;

    public void LoadScene()
    {
        SceneManager.LoadScene(Scene1);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(Scene2);
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene(Scene3);
    }

    public void ExitGame()
    {
        print("Quit....");
        Application.Quit();

    }

}
