using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StressZone : MonoBehaviour
{
    [SerializeField]public int maxItemsBeforeStress; // X items before stress starts
    public float stressIncreaseRate = 0.1f; // Stress increase per second
    private List<GameObject> objectsInZone = new List<GameObject>(); // Track objects in zone
    private bool isStressing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInZone.Contains(other.gameObject)) // Avoid duplicates
        {
            objectsInZone.Add(other.gameObject);
            CheckStressCondition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInZone.Contains(other.gameObject))
        {
            objectsInZone.Remove(other.gameObject);
            CheckStressCondition();
        }
    }

    private void CheckStressCondition()
    {
        if (objectsInZone.Count > maxItemsBeforeStress && !isStressing)
        {
            StartCoroutine(IncreaseStressOverTime());
        }
        else if (objectsInZone.Count <= maxItemsBeforeStress && isStressing)
        {
            isStressing = false;
        }
    }

    private IEnumerator IncreaseStressOverTime()
    {
        isStressing = true;
        while (isStressing && objectsInZone.Count > maxItemsBeforeStress)
        {
            StressManager.Instance.AddStress(stressIncreaseRate);
            yield return new WaitForSeconds(1f);
        }
    }
}
