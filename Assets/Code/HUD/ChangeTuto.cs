using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTuto : MonoBehaviour
{
    public Animator animator;
    public ChangeSceneMethod method;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.Play(method.animation);   
    }
}
