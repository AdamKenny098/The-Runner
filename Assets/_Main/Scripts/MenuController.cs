using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject confirmationPrompt;
    public GameObject noSaveGameDialog;
    public GameObject loadingScreen;
    public UnityEngine.UI.Image loadingBarFill;

    // === Main Buttons ===
    public void OnNewGamePressed()
    {
        GameManager.Instance.StartNewGame();
        HUDManager.Instance.ShowLoadingScreen();
        LoadingManager.Instance.LoadScene("Apartment");
    }

    public void OnContinuePressed()
    {
        GameManager.Instance.LoadGame();
        HUDManager.Instance.ShowLoadingScreen();
    }

    public void OnSettingsPressed()
    {
        HUDManager.Instance.ShowSettingsPanel();
    }

    public void OnBackPressed()
    {
        HUDManager.Instance.ShowMainMenuPanel();
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
