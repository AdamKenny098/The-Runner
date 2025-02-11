using TMPro;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float pickUpRange = 5f;
    [HideInInspector] public GameObject heldObj; // Made public so other scripts can access it
    private Rigidbody heldObjRb;

    public GameObject hoverUI;
    public GameObject hoverUIE;
    public GameObject hoverUIF; // Reference to the HoverUI GameObject
    public TMP_Text hoverText; // Reference to the TextMeshPro text component
    public LayerMask canPickUpLayer; // Layer for objects that can be picked up
    public StackManager stackManager;
    public TicketManager ticketManager;
    public GameObject heldOrder; // The order object the player is carrying.

    void Update()
    {
        HandleHoverUI();

        // Check if the player is pressing the F key (for pickup or drop actions)
        if (Input.GetKeyDown(KeyCode.F))
        {
            HandlePickUpOrDrop(); // Handles both picking up and dropping objects
        }

        // Check if the player is pressing the E key (for interactions with objects like FoodBin or LiquidBucket)
        if (Input.GetKeyDown(KeyCode.E))
        {

            HandleInteractions(); // Handles interactions with held objects
        }

        // Automatically clear heldObj if it’s no longer parented to the hold position.
        if (heldObj != null && heldObj.transform.parent != holdPos)
        {
            heldObj = null;
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
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

            if (hit.collider.CompareTag("GlassStorage"))
            {
                GlassStorageManager storageManager = hit.collider.GetComponent<GlassStorageManager>();

                if (storageManager != null)
                {
                    if (heldObj == null && storageManager.isFull)
                    {
                        Debug.Log("Player has empty hands. Storage is full, attempting to reset...");
                        storageManager.TryResetStorage();
                    }
                    else if (heldObj != null && heldObj.CompareTag("Glass") && !storageManager.isFull)
                    {
                        Debug.Log("Storing glass in storage...");
                        storageManager.StoreGlass(heldObj);
                        heldObj = null;
                    }
                    else if (heldObj != null && heldObj.CompareTag("Glass") && storageManager.isFull)
                    {
                        Debug.Log("Storage is full! You need to reset before adding more glasses.");
                    }
                    else if (heldObj == null && !storageManager.isFull)
                    {
                        Debug.Log("Storage is not full. No action needed.");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }



    void PickUpObject(GameObject pickUpObj)
    {
        heldObj = pickUpObj;
        heldObjRb = heldObj.GetComponent<Rigidbody>();
        if (heldObjRb != null)
        {
            heldObjRb.isKinematic = true;
            heldObjRb.velocity = Vector3.zero; // Reset any movement
        }

        // Parent the object to the hold position and reset its local transform
        heldObj.transform.SetParent(holdPos);
        heldObj.transform.localPosition = Vector3.zero;
        heldObj.transform.localRotation = Quaternion.identity;

        Debug.Log($"Picked up object: {heldObj.name}");
    }

    public void DropObject()
    {
        if (heldObj != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
            {
                // Check for counter interaction
                if (hit.collider.CompareTag("Counter") && heldObj.CompareTag("Plate"))
                {
                    Plate plate = heldObj.GetComponent<Plate>();
                    if (plate != null && !plate.IsDirty())
                    {
                        PlateCounter counter = hit.collider.GetComponent<PlateCounter>();
                        if (counter != null)
                        {
                            counter.AddPlateToStack(heldObj); // Add plate to counter stack
                            heldObj = null; // Reset held object
                            Debug.Log("Plate placed on counter.");
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot place dirty plate on the counter!");
                    }
                }

                if (hit.collider.CompareTag("OrderDropZone") && heldObj.CompareTag("Order"))
                {
                    Order order = heldObj.GetComponent<Order>();
                    if (order != null)
                    {
                        PlateCounter counter = hit.collider.GetComponent<PlateCounter>();
                        if (counter != null)
                        {
                            counter.AddPlateToStack(heldObj); // Add plate to counter stack
                            heldObj = null; // Reset held object
                            Debug.Log("Plate placed on counter.");
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot place dirty plate on the counter!");
                    }
                }
            }

            // If no valid target is found, drop the object normally
            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = false;
                heldObjRb.velocity = Vector3.zero;
            }
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
            UpdateHoverUI(false, false, false, "");

            if (hitObject.layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                // Check the object's specific layer for context-specific hover text
                if (hit.collider.CompareTag("Plate") && heldObj == null)
                {
                    UpdateHoverUI(true, false, true, "Pick Up Plate ?");
                }
                else if (hit.collider.CompareTag("Glass") && heldObj == null)
                {
                    UpdateHoverUI(true, false, true, "Pick Up Glass ?");
                }
                else if (hit.collider.CompareTag("Order") && heldObj == null)
                {
                    UpdateHoverUI(true, false, true, "Pick Up Order ?");
                }
                else
                {
                    UpdateHoverUI(true, false, true, "Pick Up ?");
                }
            }
            // Handle hover UI for Trash Cans
            else if (hitObject.CompareTag("TrashCan") && heldObj != null)
            {
                TrashCan trashCan = hitObject.GetComponent<TrashCan>();
                if (trashCan != null)
                {
                    if (heldObj.CompareTag(trashCan.acceptedTag))
                    {
                        UpdateHoverUI(false, true, true, "Dispose Of Object ?");
                    }
                    else
                    {
                        UpdateHoverUI(false, false, true, "Incorrect Trash Can");
                    }
                }
            }
            // Handle hover UI for food bin
            else if (hitObject.CompareTag("FoodBin") && heldObj != null)
            {
                Plate plate = heldObj.GetComponent<Plate>();
                if (plate != null && plate.IsDirty())
                {
                    UpdateHoverUI(false, true, true, "Scrape Plate?");
                }
                else
                {
                    UpdateHoverUI(false, false, false, "");
                }
            }
            // Handle hover UI for liquid bucket
            else if (hitObject.CompareTag("LiquidBucket") && heldObj != null)
            {
                Glass glass = heldObj.GetComponent<Glass>();
                if (glass != null && glass.IsDirty())
                {
                    UpdateHoverUI(false, true, true, "Empty Glass");
                }
                else
                {
                    UpdateHoverUI(false, false, false, "");
                }
            }
            // Handle hover UI for Order Counter
            else if (hitObject.CompareTag("OrderDropZone") && heldObj != null)
            {
                Order order = heldObj.GetComponent<Order>();
                if (order != null)
                {
                    UpdateHoverUI(false, true, true, "Place Order?");
                }
                else
                {
                    UpdateHoverUI(false, false, false, "");
                }
            }
            // Handle hover UI for Glass Tray
            else if (hitObject.CompareTag("GlassStorage"))
            {
                GlassStorageManager glassStorage = hitObject.GetComponent<GlassStorageManager>();

                // If the player's hand is empty and the storage is full
                if (heldObj == null && glassStorage.isFull)
                {
                    UpdateHoverUI(false, true, true, "Empty Glasses?");
                }
                // If the player is holding a glass and the storage is not full
                else if (heldObj != null)
                {
                    Glass glass = heldObj.GetComponent<Glass>();

                    if (glass != null && glassStorage.nextGlassIndex < glassStorage.glassSlots.Length)
                    {
                        UpdateHoverUI(false, true, true, "Place Glass?");
                    }
                    // If storage is full and the player is holding a glass
                    else if (glass != null && glassStorage.nextGlassIndex == glassStorage.glassSlots.Length)
                    {
                        UpdateHoverUI(false, true, true, "Storage Full, Empty it First");
                    }
                }
            }

            else
            {
                // Hide the Hover UI if no interactable object is detected
                UpdateHoverUI(false, false, false, "");
            }
        }
        else
        {
            UpdateHoverUI(false, false, false, ""); // Hide the UI if the raycast hits nothing
        }
    }

    void UpdateHoverUI(bool uiFActive, bool uiEActive, bool textActive, string message)
    {
        hoverUIF.SetActive(uiFActive);
        hoverUIE.SetActive(uiEActive);
        hoverText.gameObject.SetActive(textActive);
        if (textActive)
        {
            hoverText.text = message;
        }
    }

    void TryPickUpStack()
    {
        // Assuming you have a reference to your StackManager
        GameObject container = stackManager.CreateStackContainer();
        if (container != null)
        {
            // Use your existing pickup logic to pick up the container.
            // For example:
            PickUpObject(container);
        }
    }

    // This method is called when picking up an order.
    public void PickUpOrder(GameObject orderObj)
    {
        heldOrder = orderObj;
        Order order = orderObj.GetComponent<Order>();
        if (order != null)
        {
            // Update the ticket UI with the order details.
            ticketManager.UpdateTicket(order.GetOrderDetails());
        }
        // Additional logic to attach the order to the player, etc.
    }
}