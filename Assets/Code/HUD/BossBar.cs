using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    public Slider bossSlider;

    public float Value;

    public Animator barAnim;

    private void Update()
    {
        if(Value != bossSlider.value)
        {
            barAnim.Play("BossBarAnimation");
        }

        Value = bossSlider.value;

    }

}
