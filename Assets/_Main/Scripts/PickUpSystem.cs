using TMPro;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float pickUpRange = 5f;
    [HideInInspector] public GameObject heldObj; // Made public so other scripts can access it
    private Rigidbody heldObjRb;

    public GameObject hoverUI; // Reference to the HoverUI GameObject
    public TMP_Text hoverText; // Reference to the TextMeshPro text component
    public LayerMask canPickUpLayer; // Layer for objects that can be picked up

    void Update()
    {
        HandleHoverUI();

        // Check if the player is pressing the F key (for pickup or drop actions)
        if (Input.GetKeyDown(KeyCode.F))
        {
            HandlePickUpOrDrop(); // Handles both picking up and dropping objects
        }

        // Check if the player is pressing the E key (for interactions with objects like FoodBin or LiquidBucket)
        if (Input.GetKeyDown(KeyCode.E) && heldObj != null)
        {
            HandleInteractions(); // Handles interactions with held objects
        }
    }

    void TryPickUp()
    {
        // Raycast to detect objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange, canPickUpLayer))
        {
            GameObject target = hit.collider.gameObject;

                if (target.CompareTag("Counter"))
                {
                    PlateCounter counter = target.GetComponent<PlateCounter>();
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
                    PickUpObject(target);
                }
        }
    }

    void TryDropObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
        {
            // Drop onto the counter if valid
            if (hit.collider.CompareTag("Counter") && heldObj.layer == LayerMask.NameToLayer("Plates"))
            {
                PlateCounter counter = hit.collider.GetComponent<PlateCounter>();
                if (counter != null)
                {
                    counter.AddPlateToStack(heldObj);
                }
            }
        }

        DropObject(); // Fallback if no valid target is found
    }


    void HandlePickUpOrDrop()
    {
        if (heldObj == null)
        {
            TryPickUp();
        }
        else
        {
            TryDropObject();
        }
    }

    void HandleInteractions()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("FoodBin"))
            {
                Debug.Log("Interacting with Food Bin...");
                FoodBin foodBin = hit.collider.GetComponent<FoodBin>();
                if (foodBin != null)
                {
                    foodBin.StartScraping(heldObj.GetComponent<Plate>());
                }
            }
            else if (hit.collider.CompareTag("LiquidBucket"))
            {
                Debug.Log("Interacting with Liquid Bucket...");
                LiquidBucket liquidBucket = hit.collider.GetComponent<LiquidBucket>();
                if (liquidBucket != null)
                {
                    liquidBucket.StartCleaning(heldObj.GetComponent<Glass>());
                }
            }
            else if (hit.collider.CompareTag("LiquidBucket"))
            {
                Debug.Log("Interacting with Liquid Bucket...");
                LiquidBucket liquidBucket = hit.collider.GetComponent<LiquidBucket>();
                if (liquidBucket != null)
                {
                    liquidBucket.StartCleaning(heldObj.GetComponent<Glass>());
                }
            }
            else if (hit.collider.CompareTag("LiquidBucket"))
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


    void PickUpObject(GameObject pickUpObj)
    {
        heldObj = pickUpObj;
        heldObjRb = heldObj.GetComponent<Rigidbody>();
        if (heldObjRb != null) heldObjRb.isKinematic = true;

        // Parent the object to the hold position
        heldObj.transform.SetParent(holdPos);
        heldObj.transform.localPosition = Vector3.zero;
        heldObj.transform.localRotation = Quaternion.identity;

        Debug.Log($"Picked up object: {heldObj.name}");
    }




    public void DropObject()
    {
        if (heldObj != null)
        {
            if (heldObjRb != null) heldObjRb.isKinematic = false;

            // Unparent the object
            heldObj.transform.SetParent(null);

            Debug.Log($"Dropped object: {heldObj.name}");
            heldObj = null;
        }
    }







    void HandleHoverUI()
    {
        // Perform a raycast to detect objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange, LayerMask.GetMask("CanPickUp", "Default")))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                // Check the object's specific layer for context-specific hover text
                if (hit.collider.CompareTag("Plate") && heldObj == null)
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Pick Up Plate ?";
                }
                if (hit.collider.CompareTag("Glass") && heldObj == null)
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Pick Up Glass ?";
                }
                else
                {
                    hoverUI.SetActive(true);
                    hoverText.text = "Pick Up ?";
                }
            }
            else if (hitObject.CompareTag("Counter") && heldObj != null && heldObj.layer == LayerMask.NameToLayer("Plates"))
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