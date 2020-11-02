using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    public GameObject Boxes;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Appear();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Disappear();
        }
    }

    public void Appear()
    {
        Boxes.SetActive(true);
    }

    public void Disappear()
    {
        Boxes.SetActive(false);
    }
}
