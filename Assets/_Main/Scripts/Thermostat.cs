using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thermostat : MonoBehaviour
{

    //Seperate Map into different rooms. each are seperate scriptable Objects with their own temp value in inspector.

    public Image coldImage;
    public Image hotImage;
    public float temp = 0f;
    [Range(1,50)]public float hotTemp = 20f;
    [Range(-1,-50)]public float coldTemp = -18f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
