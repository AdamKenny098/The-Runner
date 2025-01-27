using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : MonoBehaviour
{
    public Transform[] stackPositions; // Predefined stack positions
    private Stack<GameObject> plateStack = new Stack<GameObject>();

    public void AddPlateToStack(GameObject plate)
    {
        if (plateStack.Count < stackPositions.Length)
        {
            Transform stackPosition = stackPositions[plateStack.Count];
            plate.transform.SetParent(stackPosition);
            plate.transform.localPosition = Vector3.zero; // Snap to position
            plateStack.Push(plate);
            Debug.Log($"Plate added to stack! Stack size: {plateStack.Count}");
        }
        else
        {
            Debug.Log("Counter stack is full!");
        }
    }

    public GameObject PickUpStack()
    {
        if (plateStack.Count > 0)
        {
            GameObject stack = plateStack.Pop();
            Debug.Log($"Picked up stack of plates. Remaining plates: {plateStack.Count}");
            return stack;
        }

        Debug.Log("No plates to pick up!");
        return null;
    }
}
