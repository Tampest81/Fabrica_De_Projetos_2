using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    public GameObject Boxes;

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======

>>>>>>> 42679e212155d4a59c12441abb6f2f4a32f83e20
>>>>>>> 60da8b32a4379c68d4eac87e3eab7becb50e551f
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
