using UnityEngine;

public class MistakeTrigger : MonoBehaviour
{
    [Header("Mistake Settings")]
    [SerializeField] private float mistakeStress = 10f;

    // This method is called when a mistake event occurs.
    public void OnMistakeMade()
    {
        StressManager.Instance.AddStress(mistakeStress);
    }
}
