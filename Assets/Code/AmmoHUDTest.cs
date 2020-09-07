using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AmmoHUDTest : MonoBehaviour
{
    public TextMeshProUGUI ammoDataText;
    public Image gunImage;
    public WeaponManager wpnManager;
    public Slider reloadTimerUI;

    void Start()
    {
        
    }

    void Update()
    {
        ammoDataText.text = wpnManager.guns[wpnManager.currentGunIndex]._magazineCurrent + " / " + wpnManager.guns[wpnManager.currentGunIndex]._ammoCurrent;
        gunImage.sprite = wpnManager.guns[wpnManager.currentGunIndex].gunSprite;
        reloadTimerUI.maxValue = wpnManager.guns[wpnManager.currentGunIndex]._reloadDuration;
        reloadTimerUI.value = wpnManager.guns[wpnManager.currentGunIndex]._reloadTimer;
    }
}