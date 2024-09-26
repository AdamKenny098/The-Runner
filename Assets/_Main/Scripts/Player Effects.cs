using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffects : MonoBehaviour
{
    //implement bladder ??

    [Header("Energy")]
    public Image energyImage;
    [Range(0,1)] public float energy = 1f;

    [Header("Hunger")]
    public Image hungerImage;  
    [Range(0,1)] public float hunger = 1f;  // The current hunger level, 1 is full, 0 is empty

    [Header("Thirst")]
    public Image thirstImage;  
    [Range(0,1)] public float thirst = 1f;  // The current hunger level, 1 is full, 0 is empty

    void Update()
    {
        // Update the fill amount based on the hunger value
        hungerImage.fillAmount = hunger;
        thirstImage.fillAmount = thirst;
        energyImage.fillAmount = energy;
    }

    // Method to set hunger level externally
    public void SetHunger(float value)
    {
        hunger = Mathf.Clamp01(value);  // Clamp the value between 0 and 1
    }

    public void SetThirst(float value)
    {
        thirst = Mathf.Clamp01(value);  // Clamp the value between 0 and 1
    }

    public void SetEnergy(float value)
    {
        energy = Mathf.Clamp01(value);  // Clamp the value between 0 and 1
    }


}


