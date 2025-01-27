using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] string description = "";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {

    }

    public string GetDescription()
    {
        return description;
    }

    public bool RequiresUniquePanel()
    {
        return false;
    }
}