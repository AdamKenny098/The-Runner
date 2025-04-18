using UnityEngine;

public class FogVolumeTrigger : MonoBehaviour
{
    public Color fogColor = Color.gray;
    public float fogDensity = 0.05f;
    public FogMode fogMode = FogMode.Exponential;

    private float defaultFogDensity;
    private Color defaultFogColor;
    private FogMode defaultFogMode;

    private void Start()
    {
        defaultFogDensity = RenderSettings.fogDensity;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogMode = RenderSettings.fogMode;
    }

    public void ActivateFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogStartDistance = 5f;
        RenderSettings.fogEndDistance = 8f;

    }

    public void DeactivateFog()
    {
        RenderSettings.fogDensity = defaultFogDensity;
        RenderSettings.fogColor = defaultFogColor;
        RenderSettings.fogMode = defaultFogMode;
    }
}
