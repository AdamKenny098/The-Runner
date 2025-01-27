using TMPro;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float pickUpRange = 5f;
    [HideInInspector] public GameObject heldObj; // Made public so other scripts can access it
    private Rigidbody heldObjRb;

    public GameObject hoverUI; // Reference to the HoverUI GameObject
    public TMP_Text hoverText; // Reference to the TextMeshPro text component

    void Update()
    {
        HandleHoverUI();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldObj == null)
            {
                // Raycast to detect objects to pick up or stack
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
                {
                    if (hit.collider.CompareTag("canPickUp"))
                    {
                        // Check if it's a stack on the counter
                        if (hit.collider.CompareTag("Counter"))
                        {
                            PlateCounter counter = hit.collider.GetComponent<PlateCounter>();
                            if (counter != null)
                            {
                                GameObject stack = counter.PickUpStack();
                                if (stack != null)
                                {
                                    PickUpObject(stack);
                                }
                            }
                        }
                        else
                        {
                            // Pick up the individual object
                            PickUpObject(hit.collider.gameObject);
                        }
                    }
                }
            }
            else
            {
                // Raycast to detect counter for dropping
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
                {
                    // Ensure the held object is a plate and the hit object is a counter
                    if (hit.collider.CompareTag("Counter") && heldObj.layer == LayerMask.NameToLayer("Plates"))
                    {
                        PlateCounter counter = hit.collider.GetComponent<PlateCounter>();
                        if (counter != null)
                        {
                            counter.AddPlateToStack(heldObj);
                            DropObject();
                        }
                    }
                }

                DropObject(); // Fallback if no valid drop target is found
            }
        }

        // Separate key for interactions
        if (Input.GetKeyDown(KeyCode.E) && heldObj != null)
        {
            // Raycast to detect interactable objects
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
            {
                // FoodBin Interaction
                if (hit.collider.CompareTag("FoodBin"))
                {
                    Debug.Log("Interacting with Food Bin...");
                    FoodBin foodBin = hit.collider.GetComponent<FoodBin>();
                    if (foodBin != null)
                    {
                        foodBin.StartScraping(heldObj.GetComponent<Plate>());
                    }
                }

                // LiquidBucket Interaction
                if (hit.collider.CompareTag("LiquidBucket"))
                {
                    Debug.Log("Interacting with Liquid Bucket...");
                    LiquidBucket liquidBucket = hit.collider.GetComponent<LiquidBucket>();
                    if (liquidBucket != null)
                    {
                        liquidBucket.StartCleaning(heldObj.GetComponent<Glass>());
                    }
                }
            }
        }
    }


    void PickUpObject(GameObject pickUpObj)
    {
        heldObj = pickUpObj;
        heldObjRb = heldObj.GetComponent<Rigidbody>();
        heldObjRb.isKinematic = true;

        // Cache the object's original world scale
        Vector3 originalWorldScale = heldObj.transform.lossyScale;

        // Parent the object to the hold position
        heldObj.transform.SetParent(holdPos);

        // Restore the object's world scale after parenting
        heldObj.transform.localScale = Vector3.one; // Temporarily reset local scale
        heldObj.transform.localScale = new Vector3(
            originalWorldScale.x / heldObj.transform.lossyScale.x,
            originalWorldScale.y / heldObj.transform.lossyScale.y,
            originalWorldScale.z / heldObj.transform.lossyScale.z
        );

        Debug.Log($"Picked up object: {heldObj.name}, Original World Scale: {originalWorldScale}");
    }



    void DropObject()
    {
        if (heldObj != null)
        {
            heldObjRb.isKinematic = false;
            heldObj.transform.SetParent(null);

            // Force a predefined scale (e.g., consistent for plates or objects)
            heldObj.transform.localScale = new Vector3(1, 1, 1);

            heldObj = null;
            Debug.Log("Dropped object.");
        }
    }






    void HandleHoverUI()
    {
        // Perform a raycast to detect objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("canPickUp") && heldObj == null)
            {
                // Check the object's layer for context-specific hover text
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Plates"))
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Pick Up Plate ?";
                }
                else
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Pick Up ?";
                }
            }
            else if (hit.collider.CompareTag("Counter") && heldObj != null && heldObj.layer == LayerMask.NameToLayer("Plates"))
            {
                Plate plate = heldObj.GetComponent<Plate>();
                if (plate != null && plate.IsDirty())
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Clean Plate Required!";
                }
                else
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Place Plate ?";
                }
            }

            else
            {
                // Hide the Hover UI if no interactable object is detected
                hoverUI.SetActive(false);
            }
        }
        else
        {
            hoverUI.SetActive(false); // Hide the UI if the raycast hits nothing
        }
    }
}
