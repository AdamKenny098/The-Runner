using UnityEngine;
using UnityEngine.UI;
public class PersistentCanvas : MonoBehaviour
{
    public static PersistentCanvas Instance;

    public GameObject mainMenuCanvas;
    public GameObject settingsMenuUI;
    public GameObject mainMenuUI;

    public GameObject inGameBackButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    
        mainMenuCanvas = FindObjectOfType<PersistentCanvas>().gameObject;
        settingsMenuUI = transform.GetChild(0).gameObject;
        mainMenuUI = transform.GetChild(1).gameObject;
        inGameBackButton = settingsMenuUI.transform.GetChild(0).GetChild(4).gameObject;


    }

     void Start()
    {
        // Assuming settingsMenuUI is already assigned or found
        inGameBackButton = settingsMenuUI.transform.GetChild(0).GetChild(4).gameObject;

        Button button = inGameBackButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                PauseMenu.Instance.CloseSettings();
            });
        }
    }
}
