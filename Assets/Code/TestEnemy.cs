using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpDisplay; 
    [SerializeField] public float health = 100;
    void Update()
    {
        if (health < 1) health = 100;

        hpDisplay.text = health.ToString();
    }
}
