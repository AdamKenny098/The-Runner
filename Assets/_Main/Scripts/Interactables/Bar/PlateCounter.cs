using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : MonoBehaviour
{
    public Transform[] stackPositions; // Predefined positions for stacking plates
    private Stack<GameObject> plateStack = new Stack<GameObject>(); // Stores stacked plates

    public void AddPlateToStack(GameObject plate)
    {
        if (plateStack.Count < stackPositions.Length)
        {
            // Get the next available stack position
            Transform stackPosition = stackPositions[plateStack.Count];

            // Disable physics and gravity on the plate
            Rigidbody plateRb = plate.GetComponent<Rigidbody>();
            if (plateRb != null)
            {
                plateRb.isKinematic = true; // Prevent physics interactions
                plateRb.useGravity = false; // Disable gravity
            }

            // Snap the plate to the stack position
            plate.transform.SetParent(stackPosition);
            plate.transform.localPosition = Vector3.zero; // Reset local position
            plate.transform.localRotation = Quaternion.identity; // Reset local rotation

            // Add the plate to the stack
            plateStack.Push(plate);
            Debug.Log($"Plate added to stack! Current stack size: {plateStack.Count}");
        }
        else
        {
            Debug.Log("Stack is full!");
        }
    }


    public GameObject PickUpStack()
    {
        if (plateStack.Count > 0)
        {
            // Combine all plates into one GameObject for transportation
            GameObject stackTop = plateStack.Peek(); // Get the top plate
            Transform stackParent = stackTop.transform.parent;

            foreach (GameObject plate in plateStack)
            {
                plate.transform.SetParent(stackParent); // Group all plates under one parent
            }

            Debug.Log("Picked up stack of plates!");
            plateStack.Clear(); // Clear the stack after picking up
            return stackParent.gameObject; // Return the combined stack
        }

        Debug.Log("No plates to pick up!");
        return null;
    }
}
