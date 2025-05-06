using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcScreen : MonoBehaviour
{
    public GameObject homeScreen;
    public GameObject shopScreen;
    public GameObject emailScreen;
    public GameObject bankScreen;
    public GameObject filesScreen;
    public GameObject newsScreen;

    public GameObject pcScreenCanvas;
    public GameObject pcCamera;

    public Transform playerTransform;
    public Transform playerCapsuleTransform;
    public Transform playerCameraTransform;
    public Transform playerFollowTransform;
    public CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePCScreen(bool homeActive, bool shopActive, bool emailActive, bool bankActive, bool filesActive, bool newsActive)
    {
        homeScreen.SetActive(homeActive);
        shopScreen.SetActive(shopActive);
        emailScreen.SetActive(emailActive);
        bankScreen.SetActive(bankActive);
        filesScreen.SetActive(filesActive);
        newsScreen.SetActive(newsActive);
    }

    public void HomeScreen()
    {
        UpdatePCScreen(true, false, false, false, false, false);
    }

    public void ShopScreen()
    {
        UpdatePCScreen(false, true, false, false, false, false);
    }

    public void EmailScreen()
    {
        UpdatePCScreen(false, false, true, false, false, false);
    }

    public void BankScreen()
    {
        UpdatePCScreen(false, false, false, true, false, false);
    }
    public void FilesScreen()
    {
        UpdatePCScreen(false, false, false, false, true, false);
    }
    public void NewsScreen()
    {
        UpdatePCScreen(false, false, false, false, false, true);
    }

    public void PCOff()
    {
        Debug.Log("Pc off");
        pcScreenCanvas.SetActive(false);
        pcCamera.SetActive(false);

        characterController.enabled = false;

        playerTransform.transform.position = new Vector3(-26.779f,24.2f,35.215f);
        playerCapsuleTransform.localPosition = Vector3.zero;
        playerCameraTransform.localPosition = new Vector3(0f,1.1f,0f);
        playerFollowTransform.localPosition = new Vector3(0f,1.1f,0f);

        characterController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
