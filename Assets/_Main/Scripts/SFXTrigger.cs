using UnityEngine;

public class SFXTrigger : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip sfxClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (sfxSource != null && sfxClip != null)
            {
                sfxSource.PlayOneShot(sfxClip);
                Debug.Log("Played SFX via OneShot.");
            }
            else
            {
                Debug.LogWarning("Missing AudioSource or AudioClip.");
            }
        }
    }
}
