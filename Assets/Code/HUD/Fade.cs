using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    Animator animation;

    public string Fadein;
    public string Fadein2;
    public string Fadein3;

    public string Fadeout;
    

    private void Start()
    {
        animation = GetComponent<Animator>();
        animation.Play(Fadeout);
    }

    public void fadein()
    {
        animation.Play(Fadein);
    }

    public void fadein2()
    {
        animation.Play(Fadein2);
    }

    public void fadein3()
    {
        animation.Play(Fadein3);
    }
}
