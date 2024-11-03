using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable] // Make the description class appear in the Inspector
public class Description : MonoBehaviour, IInteractable
{
    public string description;
    public GameObject descriptionPanel;
    public TextMeshProUGUI descriptionText;
    

    public void Interact()
    {
        descriptionPanel.SetActive(true);
        descriptionText.text = description;
    }

    public void CloseInteractPanel()
    {
        descriptionPanel.SetActive(false);
        descriptionText.text = ""; 
    }
}
