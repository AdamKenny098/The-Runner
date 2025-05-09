using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip errorSound;
    public AudioClip stackingSound;
    public AudioClip scrapingSound;

    public AudioClip pickUpSound;
    
    public AudioClip docketSound;
    public AudioClip canSound;
    public AudioClip garbageSound;
    public AudioClip smallSplashSound;
    public AudioClip largeSplashSound;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    public AudioSource voiceSource;

    public AudioClip traySound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayClick() => PlaySFX(clickSound);
    public void PlaySuccess() => PlaySFX(successSound);
    public void PlayError() => PlaySFX(errorSound);

    public void PlayAmbientLoop(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
}