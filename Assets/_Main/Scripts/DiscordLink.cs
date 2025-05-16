using UnityEngine;

public class DiscordLink : MonoBehaviour
{
    public string inviteURL = "https://discord.gg/kSP4YRhwb7";
    

    public void OpenDiscord()
    {
        Application.OpenURL(inviteURL);
    }
}
