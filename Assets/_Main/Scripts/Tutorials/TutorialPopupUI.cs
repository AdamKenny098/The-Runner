using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPopupUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text contentText;
    public Image tutorialImage;
    public Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePopup);
        else
            Debug.LogWarning("❌ Close button not assigned!");
    }


    // === CHEATSHEET: Set Tutorial Popup | Category: UI ===
    // NOTE: Updates popup text + image, unlocks cursor, and pauses the game
    public void SetTutorial(string title, string content, Sprite image = null)
    {
        titleText.text = title;
        contentText.text = content;

        if (image != null)
        {
            tutorialImage.sprite = image;
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    // === END ===


    // === CHEATSHEET: Close Tutorial Popup | Category: UI ===
    // NOTE: Hides popup, locks cursor again, and unpauses the game
    private void ClosePopup()
    {
        gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    // === END ===

}
