using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [Header("Stack Settings")]
    public string plateType; // Example: "Round", "Square"
    public Transform stackPoint; // Where plates will stack
    public float stackHeight = 0.2f;
    public Vector3 forcedPlateScale = new Vector3(1f, 1f, 1f);

    public GameObject stackPrefab;

    [Header("Internal References")]
    public List<GameObject> stackedPlates = new List<GameObject>();
    public Transform stackContainer;

    private void OnTriggerEnter(Collider other)
    {
        Plate plate = other.GetComponent<Plate>();
        if (plate == null || plate.plateType != plateType || plate.IsDirty()) return;

        if (stackedPlates.Contains(other.gameObject)) return;

        PickUpSystem pickUpSystem = other.GetComponentInParent<PickUpSystem>();
        if (pickUpSystem != null && pickUpSystem.heldObj == other.gameObject)
        {
            pickUpSystem.heldObj = null; // Detach from player
        }

        StackPlate(other.gameObject);
    }

    public void StackPlate(GameObject plate)
    {
        plate.transform.SetParent(stackContainer);
        plate.transform.localScale = forcedPlateScale;

        Collider col = plate.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = plate.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        Vector3 newPos = stackPoint.position + Vector3.up * stackHeight * stackedPlates.Count;
        plate.transform.position = newPos;
        plate.transform.rotation = Quaternion.identity;

        stackedPlates.Add(plate);

        TriggerTutorial();


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
        // Create a new container GameObject
        GameObject container = new GameObject("PlateStackContainer");
        container.tag = "Stack";
        container.layer = LayerMask.NameToLayer("CanPickUp"); // So it's pickable

        container.transform.position = stackPoint.position;

        // Optionally add a Rigidbody & Collider to make it interactable
        Rigidbody rb = container.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        BoxCollider col = container.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(1f, 0.05731403f, 1f); // Adjust to fit your plate stack

        // Move plates into the new container
        foreach (GameObject plate in stackedPlates)
        {
            plate.transform.SetParent(container.transform);

            // Disable individual colliders to avoid physics issues
            Collider plateCol = plate.GetComponent<Collider>();
            if (plateCol != null) plateCol.enabled = false;
        }

        stackedPlates.Clear(); // Reset the original stack
        return container;
    }


    /// <summary>
    /// Transfers all plates from another stack into this one (if types match).
    /// </summary>
    public void MergeFrom(StackManager otherStack)
    {
        if (otherStack == null || otherStack == this) return;
        if (otherStack.plateType != this.plateType) return;

        foreach (var plate in otherStack.stackedPlates)
        {
            StackPlate(plate);
        }

        otherStack.stackedPlates.Clear();
        Destroy(otherStack.gameObject);
    }

    /// <summary>
    /// Returns true if this stack has no plates.
    /// </summary>
    public bool IsEmpty()
    {
        return stackedPlates.Count == 0;
    }

    public GameObject SpawnNewStack(string plateType, Transform spawnPoint)
    {
        GameObject newStack = Instantiate(stackPrefab, spawnPoint.position, Quaternion.identity);
        StackManager manager = newStack.GetComponent<StackManager>();
        manager.plateType = plateType;
        return newStack;
    }


    private void TriggerTutorial()
    {
        
            TutorialManager.Instance.TriggerTutorial(
                "stackPlates",
                "Stacking Plates",
                "You can stack plates here to save space!",
                null
            );
        
    }
}
