using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class LockerPuzzle : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public int[] correctCode = { 3, 7, 1 };
    public LockerDial[] dials;

    [Header("Smooth Camera")]
    public Transform puzzleCamTarget;  // PuzzleCameraPoint reference
    public Transform cameraRig;        // The moving camera (your player's camera transform)
    public float transitionDuration = 0.75f;

    private bool inPuzzle = false;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;


    [Header("Visuals & Control")]
    public Animator lockerAnimator;
    public GameObject puzzleUI;
    public Cinemachine.CinemachineVirtualCamera playerCam;
    public Cinemachine.CinemachineVirtualCamera puzzleCam;

    public GameObject playerControllerRoot; // E.g., FirstPersonController script root
    public GameObject HUD; // Your player's main HUD UI (assign in Inspector)

    public bool isUnlocked = false;

    public void ActivatePuzzle()
    {
        if (inPuzzle) return;

        inPuzzle = true;
        puzzleCam.Priority = 20;  // Higher than playerCam
        playerCam.Priority = 5;

        if (HUD != null) HUD.SetActive(false);
        if (playerControllerRoot != null) playerControllerRoot.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        puzzleUI.SetActive(true);
    }



    


    public void ExitPuzzle()
    {
        puzzleUI.SetActive(false);
        puzzleCam.Priority = 5;
        playerCam.Priority = 20;

        if (HUD != null) HUD.SetActive(true);
        if (playerControllerRoot != null) playerControllerRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inPuzzle = false;
    }

    public void CheckCode()
    {
        if (isUnlocked) return;

        for (int i = 0; i < correctCode.Length; i++)
        {
            if (dials[i].currentValue != correctCode[i])
                return;
        }

        UnlockLocker();
    }

    private void UnlockLocker()
    {
        isUnlocked = true;
        ExitPuzzle(); // Hide UI & return control
        lockerAnimator.SetTrigger("Open");
        Debug.Log("✅ Locker Unlocked!");
    }
}
