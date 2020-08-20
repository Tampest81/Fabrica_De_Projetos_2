using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private int health = 3;
    void Update()
    {
        switch (health)
        {
            case 3:
                this.GetComponent<SpriteRenderer>().material.color = Color.red;
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().material.color = Color.yellow;
                break;
            case 1:
                this.GetComponent<SpriteRenderer>().material.color = Color.green;
                break;
            case 0:
                health = -1;
                this.GetComponent<SpriteRenderer>().material.color = Color.clear;
                Invoke("ResetEnemy", 1);
                break;
        }
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
    }
    private void ResetEnemy()
    {
        health = 3;
    }
}
