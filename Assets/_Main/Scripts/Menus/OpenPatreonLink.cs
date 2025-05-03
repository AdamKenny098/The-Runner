using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPatreonLink : MonoBehaviour
{
    // Make sure you fill this in with your Patreon link
    [SerializeField] private string patreonURL = "https://www.patreon.com/c/098_Forge";

    // Public method to open the link
    public void OpenLink()
    {
        Application.OpenURL(patreonURL);
    }
}
