using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StressJumpscare : MonoBehaviour
{
    public Image jumpscareImage; // Assign the UI Image in Inspector
    public AudioClip jumpscareSound; // Assign a jumpscare sound
    public float flashDuration = 4f;
    public float fadeDuration = 4.5f;
    public float cameraShakeDuration = 0.7f;
    public float cameraShakeIntensity = 0.2f;

    private AudioSource audioSource;
    private Camera mainCamera;
    private Vector3 originalCamPos;

    public bool hasTriggered = false; // Prevents multiple executions

    public GameOverManager gameOverManager;

    void Start()
    {
        // Ensure the UI Image is disabled at the start
        if (jumpscareImage != null)
            jumpscareImage.color = new Color(0, 0, 0, 0);

        // Set up Audio Source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = jumpscareSound;
        audioSource.playOnAwake = false;

        // Get Camera
        mainCamera = Camera.main;
        if (mainCamera != null)
            originalCamPos = mainCamera.transform.localPosition;
    }

    public void TriggerJumpscare()
    {
        if (!hasTriggered) // Ensures it only executes once
        {
            hasTriggered = true;
            gameOverManager.DisableArray();
            StartCoroutine(JumpscareSequence());
        }
    }

    private IEnumerator JumpscareSequence()
    {
        // **Step 1: Flash Red Instantly**
        if (jumpscareImage != null)
        {
            jumpscareImage.color = new Color(1, 0, 0, 1); // Full Red
        }

        // Play jumpscare sound
        if (audioSource != null && jumpscareSound != null)
        {
            audioSource.Play();
        }

        // Start camera shake
        StartCoroutine(CameraShake());

        // **Step 2: Hold Red for 1.5 seconds**
        yield return new WaitForSeconds(1.5f);

        // **Step 3: Gradually Fade from Red to Black Over 3 Seconds**
        float elapsedTime = 0f;
        float fadeToBlackDuration = 3.0f;

        while (elapsedTime < fadeToBlackDuration)
        {
            elapsedTime += Time.deltaTime;
            if (jumpscareImage != null)
            {
                float alpha = Mathf.Lerp(1, 1, elapsedTime / fadeToBlackDuration); // Ensures full black at the end
                jumpscareImage.color = new Color(0, 0, 0, alpha);
            }
            yield return null;

        }


        // **Step 4: Keep the Black Screen for 4 Seconds**
        if (jumpscareImage != null)
        {
            jumpscareImage.color = new Color(0, 0, 0, 1); // Ensures full black stays
        }
        yield return new WaitForSeconds(4f);
        gameOverManager.EnableGameOver();
        this.gameObject.SetActive(false);
    }



    private IEnumerator CameraShake()
    {
        float elapsed = 0;
        while (elapsed < cameraShakeDuration)
        {
            elapsed += Time.deltaTime;
            if (mainCamera != null)
            {
                Vector3 randomOffset = Random.insideUnitSphere * cameraShakeIntensity;
                mainCamera.transform.localPosition = originalCamPos + randomOffset;
            }
            yield return null;
        }

        // Reset camera position
        if (mainCamera != null)
            mainCamera.transform.localPosition = originalCamPos;
    }
}
