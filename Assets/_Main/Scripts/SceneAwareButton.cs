using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAwareButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "MainMenu":
                DoActionA();
                break;

            default:
                DoActionB();
                break;
        }
    }

    void DoActionA()
    {
        PersistentCanvas.Instance.mainMenuUI.SetActive(true);
        PersistentCanvas.Instance.settingsMenuUI.SetActive(false);
    }

    void DoActionB()
    {
        PauseMenu.Instance.CloseSettings();
    }
}
