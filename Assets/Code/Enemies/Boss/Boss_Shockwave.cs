using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shockwave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(1, true);
        }
    }
}
