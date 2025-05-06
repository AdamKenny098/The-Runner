using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance;
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button settingsButton;
    public Button menuButton;
    public GameObject settingsMenuUI;
    public GameObject player;
    public GameObject objectToDisable;
    public string[] targetScenes; // Add scene names in Inspector

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        objectToDisable = GameObject.Find("PauseMenuUICanvas");
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Main Menu" || currentScene == "y")
        {
            if (objectToDisable != null)
                objectToDisable.SetActive(false);
        }

        else
        {
            objectToDisable.SetActive(true);
        }

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }

            else
            {
                PauseGame();
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (string sceneName in targetScenes)
        {
            if (scene.name == sceneName && objectToDisable != null)
            {
                objectToDisable.SetActive(false);
                Cursor.lockState = CursorLockMode.None; // Lock the cursor to the centre of the screen
                Cursor.visible = true; // Make the cursor invisible
                return;
            }

            else
            {
                objectToDisable.SetActive(true);
                if(pauseMenuUI != null)
                {
                    pauseMenuUI.SetActive(false);
                }

                else 
                {
                    return;
                }
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the centre of the screen
                Cursor.visible = false; // Make the cursor invisible
            }
        }

        // Optionally re-enable it in other scenes
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(true);
        }
    }


    public void Resume()
    {
        if (player != null)
        {
            player.SetActive(true);
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        if (player != null)
        {
            player.SetActive(false);
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Settings()
    {
        settingsMenuUI.SetActive(true);
    }

    public void MainMenu()
    {
        SaveSystem.SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
