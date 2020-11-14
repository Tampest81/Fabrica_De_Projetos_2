using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlider : MonoBehaviour
{
    public Slider bossSlider;
    public Boss_Attacks bossScript;

    private void Start()
    {
        bossSlider.maxValue = bossScript.bossHp;
    }

    private void Update()
    {
        bossSlider.value = bossScript.bossHp;
    }
}
