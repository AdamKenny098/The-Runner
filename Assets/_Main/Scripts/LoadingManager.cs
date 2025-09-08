using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [Header("Loading Screen")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text tipText;

    [Header("Tips")]
    [TextArea(2, 4)]
    [SerializeField] private string[] tips;
    [SerializeField] private float tipChangeInterval = 4f;
    [SerializeField] private float fadeDuration = 0.5f;

    private string lastScene = ""; // 🆕 track where we came from

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (loadingCanvasGroup != null)
        {
            loadingCanvasGroup.alpha = 0f;
            loadingCanvasGroup.gameObject.SetActive(false);
        }
    }

    private IEnumerator HandleLoadingScreen(string sceneName)
    {
        // Fade in loading screen
        if (loadingCanvasGroup != null)
        {
            loadingCanvasGroup.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(loadingCanvasGroup, 0f, 1f, fadeDuration));
        }

        // Start cycling tips
        if (tipText != null && tips.Length > 0)
            StartCoroutine(CycleTips());

        // Begin async load
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            if (progressBarFill != null)
                progressBarFill.fillAmount = progress;

            if (progressText != null)
                progressText.text = (progress * 100f).ToString("0") + "%";

            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }

        // Fade out after load finishes
        if (loadingCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(loadingCanvasGroup, 1f, 0f, fadeDuration));
            loadingCanvasGroup.gameObject.SetActive(false);
        }
    }

    private IEnumerator CycleTips()
    {
        while (loadingCanvasGroup != null && loadingCanvasGroup.gameObject.activeSelf)
        {
            int randomIndex = Random.Range(0, tips.Length);
            tipText.text = tips[randomIndex];

            yield return new WaitForSeconds(tipChangeInterval);
        }
    }

    // Fades any CanvasGroup
    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float t = 0f;
        group.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        group.alpha = to;
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(HandleLoadingScreen(sceneName));
    }
}
