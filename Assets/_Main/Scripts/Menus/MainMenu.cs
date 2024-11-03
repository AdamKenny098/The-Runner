using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public string newGame;
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    public void NewGame()
    {
        LoadScene();
    }

    public void ContinueGame()
    {

    }

    public void LoadGame()
    {

    }

    public void Options()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
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
