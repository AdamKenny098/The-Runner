using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// -------------------------------------------------

// Author: Adam Kenny
// Student: Applied Computing (Game Development) 3rd Year (20102588)
// Date Created: 2025-08-15
// Description: Manages the Heads-Up Display (HUD) in the game, including toggling visibility of various UI elements like instructions, countdown timer, task prompts, and day count.

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public GameObject hudRootObject;

    [Header("HUD Children")]
    public GameObject interactHUD;
    public GameObject UIKeyBindPanel;
    public GameObject sessionTimer;
    public GameObject playerNeedsBars;
    public GameObject stressUI;

    public GameObject inventoryUICanvas;
    public GameObject pauseMenuUICanvas;

    [Header("Overlay Panels")]
    public GameObject pauseMenuOverlay;
    public GameObject settingsOverlay;
    public GameObject loadingScreen;
    public GameObject playerMenuOverlay;

    public GameObject gameOverCanvas;
    public GameObject stressJumpscare;

    [Header("Main Menu Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public Image loadingBarFill;
    public bool InventoryLocked = false;
    public void SetInventoryLocked(bool locked)
    {
        InventoryLocked = locked;
    }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Objects that transcend scenes
        GameObject GameManager = GameObject.Find("GameManager");
        pauseMenuUICanvas = GameObject.Find("PauseMenuUICanvas");
        pauseMenuOverlay = pauseMenuUICanvas.transform.GetChild(0).gameObject;
        settingsOverlay = pauseMenuUICanvas.transform.GetChild(1).gameObject;
        GameObject settingsPopOutPartent = pauseMenuUICanvas.transform.GetChild(2).gameObject;
        GameObject GraphicsSettings = settingsPopOutPartent.transform.GetChild(0).gameObject;
        GameObject AudioSettings = settingsPopOutPartent.transform.GetChild(1).gameObject;
        GameObject GampeplaySettings = settingsPopOutPartent.transform.GetChild(2).gameObject;

        loadingScreen = GameManager.transform.GetChild(1).gameObject;

        if (scene.name == "Apartment" || scene.name == "Bar") // Or check for other game scenes as needed
        {
            hudRootObject = GameObject.Find("HUD");
            interactHUD = hudRootObject.transform.GetChild(1).gameObject;
            UIKeyBindPanel = hudRootObject.transform.GetChild(0).gameObject;
            sessionTimer = hudRootObject.transform.GetChild(2).gameObject;
            playerNeedsBars = hudRootObject.transform.GetChild(3).gameObject;
            stressUI = hudRootObject.transform.GetChild(4).gameObject;


            gameOverCanvas = hudRootObject.transform.GetChild(5).gameObject;
            stressJumpscare = hudRootObject.transform.GetChild(6).gameObject;
            playerMenuOverlay = hudRootObject.transform.GetChild(9).gameObject;
        }

        else if (scene.name == "Main Menu")
        {
            // Assign references dynamically
            GameObject gameManager = GameObject.Find("GameManager");
            GameObject mainPanelParent = GameObject.Find("MainMenuCanvas");
            mainPanel = mainPanelParent.transform.GetChild(0).gameObject;
            settingsPanel = gameManager.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

            //GameObject confirmationPrompt = mainPanelParent.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject;
            //GameObject noSaveGameDialog = mainPanelParent.transform.GetChild(1).gameObject.transform.GetChild(4).gameObject;
        }
    }


    public void Start()
    {
        ShowDefaultHUD();
        HideAllOverlays();
    }

    public void SetHUDChildren(bool showinteractHUD, bool showUIKeyBindPanel, bool showSessionTimer, bool showplayerNeedsBars, bool showstressUI)
    {
        if (interactHUD) interactHUD.SetActive(showinteractHUD);
        if (UIKeyBindPanel) UIKeyBindPanel.SetActive(showUIKeyBindPanel);
        if (sessionTimer) interactHUD.SetActive(showSessionTimer);
        if (playerNeedsBars) playerNeedsBars.SetActive(showplayerNeedsBars);
        if (stressUI) stressUI.SetActive(showstressUI);
    }

    public void HideAllHUD()
    {
        SetHUDChildren(false, false, false, false, false);
    }

    public void ShowDefaultHUD()
    {
        SetHUDChildren(false, false, true, true, true);
    }

    public void ShowinteractHUD()
    {
        SetHUDChildren(true, false, false, false, false);
    }

    public void ShowInstructions()
    {
        SetHUDChildren(false, true, false, false, false);
    }

    public void HideAllOverlays()
    {
        if (pauseMenuOverlay) pauseMenuOverlay.SetActive(false);
        if (settingsOverlay) settingsOverlay.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        SetInventoryLocked(true);
        HideAllOverlays();
        if (pauseMenuOverlay) pauseMenuOverlay.SetActive(true);
        HideAllHUD();
        SetPausedState(true);
    }

    public void ClosePauseMenu()
    {
        HideAllOverlays();
        ShowDefaultHUD();
        SetPausedState(false);

        SetInventoryLocked(false);
    }

    public void OpenSettings()
    {
        SetInventoryLocked(true);
        if (pauseMenuOverlay) pauseMenuOverlay.SetActive(false);
        if (settingsOverlay) settingsOverlay.SetActive(true);

    }

    public void CloseSettingsToPause()
    {
        SetInventoryLocked(true);
        if (settingsOverlay) settingsOverlay.SetActive(false);
        if (pauseMenuOverlay) pauseMenuOverlay.SetActive(true);


    }

    // public void OpenPlayerInventory()
    // {
    //     HideAllOverlays();
    //     if (playerInventoryOverlay) playerInventoryOverlay.SetActive(true);
    //     upgradeUI.SetActive(true);
    //     HideAllHUD();
    //     SetPausedState(true);
    // }

    // public void CloseInventory()
    // {
    //     if (playerInventoryOverlay) playerInventoryOverlay.SetActive(false);
    //     if (otherInventoryOverlay) otherInventoryOverlay.SetActive(false);
    //     if (inventoryCloseButton) inventoryCloseButton.SetActive(false);
    //     if (upgradeUI) upgradeUI.SetActive(false);
    //     ShowDefaultHUD();
    //     SetPausedState(false);
    // }


    private void SetPausedState(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // --- MAIN MENU UI --- //
    public void ShowMainMenuPanel()
    {
        HideAllOverlays();
        if (mainPanel) mainPanel.SetActive(true);
    }

    public void ShowSettingsPanel()
    {
        HideAllOverlays();
        if (settingsPanel) settingsPanel.SetActive(true);
        if (mainPanel) mainPanel.SetActive(false);
    }

    public void ShowLoadingScreen(float progress = 0f)
    {
        if (loadingScreen) loadingScreen.SetActive(true);
        if (loadingBarFill) loadingBarFill.fillAmount = progress;
    }

    public void HideLoadingScreen()
    {
        if (loadingScreen) loadingScreen.SetActive(false);
    }



}
