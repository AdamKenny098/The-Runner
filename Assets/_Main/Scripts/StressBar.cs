using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{

    public Slider stressSlider;
    public Slider yellowSlider;
    public float maxStress = 100f;
    public float stress;
    public float dmgSpeed = .05f;
    public bool burnUp = false;

    void Start()
    {
        stress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        BurnUpStage();
        if(stressSlider.value != stress)
        {
            stressSlider.value = stress;
        }


        if(stressSlider.value != yellowSlider.value)
        {
            yellowSlider.value = Mathf.Lerp(yellowSlider.value, stress, dmgSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        if(burnUp == true)
        {
            stress += (damage *0.75f);
        }
        else
        {
            stress += damage;
        }
        
    }

    public void BurnUpStage()
    {
        if(stress > 75)
        {
            burnUp = true;
        }
        else
        {
            burnUp = false;
        }
    }
}
