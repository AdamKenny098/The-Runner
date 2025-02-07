using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ReadmeOpener
{
    private const string sessionKey = "ReadmeOpened";  // Key to track if README has opened

    static ReadmeOpener()
    {
        // Check if the README has already been opened in this session
        if (!SessionState.GetBool(sessionKey, false))
        {
            OpenReadme();
            SessionState.SetBool(sessionKey, true);  // Mark as opened
        }
    }

    private static void OpenReadme()
    {
        string readmePath = "Assets/README.txt";  // Change path if needed
        TextAsset readmeFile = AssetDatabase.LoadAssetAtPath<TextAsset>(readmePath);

        if (readmeFile != null)
        {
            AssetDatabase.OpenAsset(readmeFile);
            Debug.Log("README opened on project load.");
        }
        else
        {
            Debug.LogWarning("README file not found at: " + readmePath);
        }
    }
}
