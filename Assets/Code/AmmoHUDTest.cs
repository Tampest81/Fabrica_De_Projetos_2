using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class AmmoHUDTest : MonoBehaviour
{
    public TextMeshProUGUI ammoDataText;
    public WeaponManager wpnManager;
    void Start()
    {
        
    }

    void Update()
    {
        ammoDataText.text = wpnManager.magazineCurrent + " / " + wpnManager.ammoReservesTotal;
    }
}
