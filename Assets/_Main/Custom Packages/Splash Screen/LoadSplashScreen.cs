using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSplashScreen : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main Menu"; // Set your main scene name in the Inspector
    [SerializeField] private float splashDuration = 3f; // Duration of the splash screen in seconds

    private void Start()
    {
        StartCoroutine(LoadMainSceneAfterDelay());
    }

    private IEnumerator LoadMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(splashDuration);

        SceneManager.LoadScene(mainSceneName);
    }
}
