using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // UI Sounds
    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip errorSound;

    // Interaction Sounds
    public AudioClip pickUpSound;
    public AudioClip stackingSound;
    public AudioClip scrapingSound;
    public AudioClip docketSound;
    public AudioClip canSound;
    public AudioClip garbageSound;

    // Splash Sounds
    public AudioClip smallSplashSound;
    public AudioClip largeSplashSound;

    // Tray Sounds
    public AudioClip traySound;

    // Audio Sources
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    public AudioSource voiceSource;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensures this object persists across scenes
        }
        else 
        {
            Destroy(gameObject); // Destroy if another instance exists
        }
    }

    // Play a one-shot SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // UI SFX Methods
    public void PlayClick() => PlaySFX(clickSound);
    public void PlaySuccess() => PlaySFX(successSound);
    public void PlayError() => PlaySFX(errorSound);

    // Ambient Methods
    public void PlayAmbientLoop(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    // Music Methods
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // Specific Sounds for Interactions
    public void PlayPickUp() => PlaySFX(pickUpSound);
    public void PlayStacking() => PlaySFX(stackingSound);
    public void PlayScraping() => PlaySFX(scrapingSound);
    public void PlayDocket() => PlaySFX(docketSound);
    public void PlayCanSound() => PlaySFX(canSound);
    public void PlayGarbageSound() => PlaySFX(garbageSound);
    public void PlaySmallSplash() => PlaySFX(smallSplashSound);
    public void PlayLargeSplash() => PlaySFX(largeSplashSound);

    // Tray Sound
    public void PlayTraySound() => PlaySFX(traySound);
}
