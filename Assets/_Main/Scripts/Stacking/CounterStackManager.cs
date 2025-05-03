using UnityEngine;
using System.Collections.Generic;


public class CounterStackManager : MonoBehaviour
{
    // Array of predefined positions for stacks on the counter.
    public StackSlot[] stackSlots; // Make sure to assign 5 positions in the Inspector.

    // Optionally track which positions are already occupied.
    // For a simple approach, we'll use an index to assign positions sequentially.
    private int nextStackIndex = 0;
    public float washInterval = 3f;

    private void Start()
    {
        InvokeRepeating(nameof(WashDish), washInterval, washInterval);
    }

    public void AcceptStack(GameObject other)
    {
        if (other.CompareTag("Stack"))
        {
            for (int i = 0; i < stackSlots.Length; i++)
            {
                if (stackSlots[i].stackObject == null)
                {
                    Transform targetPoint = stackSlots[i].position;
                    other.transform.position = targetPoint.position;
                    other.transform.rotation = Quaternion.identity;
                    other.transform.localScale = Vector3.one;
                    other.transform.SetParent(targetPoint);

                    stackSlots[i].stackObject = other;

                    // ✅ Move the tutorial trigger INSIDE the loop, before return
                    DeliverStackTutorial();

                    return;
                }
            }

            Debug.Log("No available stack positions on the counter.");
        }
    }


    public void WashDish()
{
    for (int i = 0; i < stackSlots.Length; i++)
    {
        GameObject stackObj = stackSlots[i].stackObject;

        if (stackObj != null)
        {
            if (stackObj.transform.childCount > 0)
            {
                Transform topPlate = stackObj.transform.GetChild(stackObj.transform.childCount - 1);

                Plate plate = topPlate.GetComponent<Plate>();
                if (plate != null)
                {
                    int score = GetScoreForPlateType(plate.plateType);
                    ScoreManager.Instance.AddScore(score);
                    Debug.Log($"✅ Washed {plate.plateType} plate. +{score} points.");
                }

                Destroy(topPlate.gameObject);

                if (stackObj.transform.childCount == 0)
                {
                    Destroy(stackObj);
                    stackSlots[i].stackObject = null;
                }

                return;
            }
            else
            {
                Destroy(stackObj);
                stackSlots[i].stackObject = null;
            }
        }
    }
}


    private int GetScoreForPlateType(string plateType)
    {
        switch (plateType)
        {
            case "Starter": return 10;
            case "Entree": return 20;
            case "Bowl": return 15;
            default: return 5;
        }
    }


    private void DeliverStackTutorial()
    {
            TutorialManager.Instance.TriggerTutorial(
                "deliveredPlateStack",
                "Delivering Plates",
                "Cleaned plates go here to be washed automatically. Good job!",
                null
            );
    }

    // Optional: If you ever need to remove a container (or free a position),
    // you can implement a method that decreases nextStackIndex or marks a specific slot as free.
}

