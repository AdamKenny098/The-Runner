using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootUpScript : MonoBehaviour
{

    void Awake()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1); // Max quality
    }


    // Start is called before the first frame update
    void Start()
    {
        // Set to native resolution
        int screenWidth = Display.main.systemWidth;
        int screenHeight = Display.main.systemHeight;

        Screen.SetResolution(screenWidth, screenHeight, FullScreenMode.FullScreenWindow);
        QualitySettings.antiAliasing = 4; // Optional: ensure AA is on
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
