using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Settings")]
    public string acceptedTag; // The tag this trash can accepts (e.g., "Recycling" or "Waste")
    public float disposalTime = 0.5f; // Time it takes to dispose of the object

    private bool isInteracting = false;

    private void OnTriggerStay(Collider other)
    {
        // Check if the player is holding an object
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && other.CompareTag("Player"))
        {
            PickUpSystem pickUpSystem = other.GetComponent<PickUpSystem>();
            if (pickUpSystem != null && pickUpSystem.heldObj != null)
            {
                // Check if the held object has the correct tag
                if (pickUpSystem.heldObj.CompareTag(acceptedTag))
                {
                    StartCoroutine(DisposeObject(pickUpSystem));
                }
                else
                {
                    Debug.Log("Incorrect trash can for this object!");
                }
            }
        }
    }

    private IEnumerator DisposeObject(PickUpSystem pickUpSystem)
    {
        isInteracting = true;

        Debug.Log("Disposing of object...");
        yield return new WaitForSeconds(disposalTime);

        // Delete the object
        Destroy(pickUpSystem.heldObj);

        // Reset held object in the pick-up system
        pickUpSystem.DropObject();

        Debug.Log("Object disposed of!");
        isInteracting = false;
    }
}
