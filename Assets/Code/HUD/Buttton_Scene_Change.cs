using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttton_Scene_Change : MonoBehaviour
{
    public string scenename;

    Animator animation;
    public string animationName;

    private void Start()
    {
        animation = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            animation.Play(animationName);
        }
    }

    public void sceneChange()
    {
        SceneManager.LoadScene(scenename);
    }
}
