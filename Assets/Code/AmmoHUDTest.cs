using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHUDTest : MonoBehaviour
{
    public TextMeshProUGUI ammoDataText;
    public WeaponManager wpnManager;
    public Slider reloadTimerUI;

    void Start()
    {
        
    }

    void Update()
    {
        ammoDataText.text = wpnManager.magazineCurrent + " / " + wpnManager.ammoReservesTotal;

        reloadTimerUI.maxValue = wpnManager.reloadTime;
        reloadTimerUI.value = wpnManager.reloadTimer;
    }
}
