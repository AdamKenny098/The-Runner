using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;

    public void NewGame()
    {

    }

    public void ContinueGame()
    {

    }

    public void LoadGame()
    {

    }

    public void Options()
    {
        bool settingsEnabled = settingsMenu.activeSelf;
        bool mainEnabled = mainMenu.activeSelf;
        settingsMenu.SetActive(!settingsEnabled);
        mainMenu.SetActive(!mainEnabled);
    }


    public void Credits()
    {

    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
