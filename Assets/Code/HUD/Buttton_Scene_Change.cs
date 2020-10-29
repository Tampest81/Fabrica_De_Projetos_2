using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttton_Scene_Change : MonoBehaviour
{
    public string scenename;

    Animator animation;

    public string StartAnim;
    public string animationName;
    public string DisappearClip;
    public string Loadinganim;


    public string FadeIn;
    public string FadeOut;

    private void Start()
    {
        animation = GetComponent<Animator>();
        animation.Play(FadeOut);
        
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

    public void ButtonDisappear()
    {
        animation.Play(DisappearClip);
    }

    public void LoadingAnim()
    {
        animation.Play(Loadinganim);
    }

    public void Fadein()
    {
        animation.Play(FadeIn);
    }

    public void Startanim()
    {
        animation.Play(StartAnim);
    }
}
