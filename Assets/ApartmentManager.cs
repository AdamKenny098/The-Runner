using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApartmentManager : MonoBehaviour
{
    [SerializeField] private GameObject computerInApartment;
    [SerializeField] private GameObject brokenTVStand;
    [SerializeField] private GameObject brokenKitchen;
    [SerializeField] private GameObject fixedTVStand;
    [SerializeField] private GameObject fixedKitchen;

    [SerializeField] private GameObject loadingScreen;
    public GameManager gameManager;

    public GameObject welcomePanel;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
            return;
        }

        if (GameManager.Instance.hasBoughtComputer)
        {
            computerInApartment.SetActive(true);
        }

        if(GameManager.Instance.hasRepairedTVStand)
        {
            brokenTVStand.SetActive(false);
            fixedTVStand.SetActive(true);
        }
        else
        {
            brokenTVStand.SetActive(true);
            fixedTVStand.SetActive(false);
        }

        if(GameManager.Instance.hasRepairedKitchen)
        {
            brokenKitchen.SetActive(false);
            fixedKitchen.SetActive(true);
        }
        else
        {
            brokenKitchen.SetActive(true);
            fixedKitchen.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        CheckTagInteractions();
    }

    private void CheckTagInteractions()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f)) // Adjust interaction range as needed
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject target = hit.collider.gameObject;

                switch (target.tag)
                {
                    case "FrontDoor":
                    Debug.Log("Front Door E");
                        FrontDoorHandle doorHandle = target.GetComponent<FrontDoorHandle>();
                        if (doorHandle != null)
                        {
                            loadingScreen.SetActive(true);

                            SceneManager.LoadScene("Bar"); // Replace with correct scene name
                        }
                        break;

                    case "Computer":
                        // Example: Turn on computer
                        
                        break;

                    case "TVStand":
                        // Example: Prompt repair option
                        Debug.Log("Interacting with TV stand...");
                        break;

                    case "Kitchen":
                        // Example: Show kitchen repair status
                        Debug.Log("Interacting with kitchen...");
                        break;

                    default:
                        Debug.Log("No interaction set for tag: " + target.tag);
                        break;
                }
            }
        }
    }

    public void CloseWelcomePanel()
    {
        welcomePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
