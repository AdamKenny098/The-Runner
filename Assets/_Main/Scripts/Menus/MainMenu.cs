using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    private string newGame = "Apartment";
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void NewGame()
    {
        gameManager.StartNewGame();
        LoadScene();
    }

    public void ContinueGame()
    {
        SaveSystem.LoadGame();
        LoadScene();
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


    private void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(newGame);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
}
