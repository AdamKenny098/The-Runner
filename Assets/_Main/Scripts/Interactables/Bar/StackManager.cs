using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public string plateType; // Example: "Round", "Square"
    public Transform stackPoint; // Position where plates will stack
    public float stackHeight = 0.2f; // Distance between stacked plates
    public List<GameObject> stackedPlates = new List<GameObject>();
    public Transform stackContainer;

    // Define the forced scale for plates when stacked.
    public Vector3 forcedPlateScale = new Vector3(1f, 1f, 1f);

    private void OnTriggerEnter(Collider other)
    {
        Plate plate = other.GetComponent<Plate>();

        if (plate != null && plate.plateType == plateType && !plate.IsDirty()) // Check if it's a matching plate
        {
            // If this plate is already in the stack, do nothing.
            if (stackedPlates.Contains(other.gameObject))
                return;

            // If the plate is being held by a player's PickUpSystem, clear it.
            PickUpSystem pickUpSystem = other.GetComponentInParent<PickUpSystem>();
            if (pickUpSystem != null && pickUpSystem.heldObj == other.gameObject)
            {
                // Option 1: Call a method to drop the plate (if available)
                // pickUpSystem.DropObject(); 

                // Option 2: Directly clear the held object reference.
                pickUpSystem.heldObj = null;
            }

            StackPlate(other.gameObject);
        }
    }


    public void StackPlate(GameObject plate)
    {
        plate.transform.SetParent(stackContainer);

        // Force the plate's scale to be the desired value.
        plate.transform.localScale = forcedPlateScale;

        // Optionally disable the plate's collider to prevent unwanted physics interactions
        Collider plateCollider = plate.GetComponent<Collider>();
        if (plateCollider != null)
            plateCollider.enabled = false;

        // If the plate has a Rigidbody, set it to kinematic
        Rigidbody plateRb = plate.GetComponent<Rigidbody>();
        if (plateRb != null)
            plateRb.isKinematic = true;

        int stackCount = stackedPlates.Count;

        Vector3 newPos = stackPoint.position + new Vector3(0, stackHeight * stackCount, 0);
        plate.transform.position = newPos;
        plate.transform.rotation = Quaternion.identity;

        stackedPlates.Add(plate);
    }

    public void RemoveStack()
    {
        foreach (GameObject plate in stackedPlates)
        {
            plate.transform.SetParent(null);
        }
        stackedPlates.Clear();
    }
    public GameObject CreateStackContainer()
    {
        // Create a new empty GameObject to serve as the container
        GameObject container = new GameObject("PlateStackContainer");
        container.tag = "Stack";
        container.layer = 13;

        // Position the container at the current stack point (or another appropriate location)
        container.transform.position = stackPoint.position;

        // Optional: Add a collider and Rigidbody to the container if needed for pickup interactions.
        // For example:
        // BoxCollider containerCollider = container.AddComponent<BoxCollider>();
        // Rigidbody containerRb = container.AddComponent<Rigidbody>();
        // containerRb.isKinematic = true;

        // Reparent each plate in the stack to the container
        foreach (GameObject plate in stackedPlates)
        {
            plate.transform.SetParent(container.transform);

            // Optionally disable individual colliders to prevent unwanted physics interactions.
            Collider plateCollider = plate.GetComponent<Collider>();
            if (plateCollider != null)
                plateCollider.enabled = false;
        }

        // Clear the stacked plates list since they are now children of the container.
        stackedPlates.Clear();

        return container;
    }

}
