using TMPro;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public Transform trayHoldPos;   // Custom hold position for the tray
    public float pickUpRange = 5f;
    public GameObject heldObj; // Made public so other scripts can access it
    private Rigidbody heldObjRb;

    public GameObject hoverUI;
    public GameObject hoverUIE;
    public GameObject hoverUIF; // Reference to the HoverUI GameObject
    public TMP_Text hoverText; // Reference to the TextMeshPro text component
    public LayerMask canPickUpLayer; // Layer for objects that can be picked up

    public TrayManager trayManager;
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
        if (heldObj != null && (heldObj.transform.parent != holdPos && heldObj.transform.parent != trayHoldPos))
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
            Debug.Log(hit.collider.name);
            // If the held object is a tray, handle tray interactions.
            if (hit.collider.CompareTag("TrayPosition"))
            {
                if (heldObj != null && heldObj.CompareTag("Tray"))
                {
                    TrayManager trayManager = heldObj.GetComponent<TrayManager>();
                    if (trayManager != null)
                    {
                        trayManager.PlaceTray(heldObj, hit.transform);

                    }
                }
                else if (heldObj == null && hit.transform.childCount > 0)
                {
                    Transform firstChild = hit.transform.GetChild(0);

                    if (firstChild.CompareTag("Tray"))
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            PickUpObject(firstChild.gameObject);
                        }
                    }
                }
            }

            else if (heldObj != null && heldObj.CompareTag("Tray"))
            {
                OrderDropZone orderDropZone = hit.collider.GetComponent<OrderDropZone>();
                if (orderDropZone != null)
                {
                    // Call the tray-specific drop logic.
                    TrayManager trayManager = heldObj.GetComponent<TrayManager>();
                    if (trayManager != null)
                    {
                        orderDropZone.HandleTrayDrop(trayManager);
                        return; // Exit after handling tray interaction.
                    }
                }
            }
            // Otherwise, continue with the regular interactions.
            else if (hit.collider.CompareTag("GlassStorage"))
            {
                GlassStorageManager storageManager = hit.collider.GetComponent<GlassStorageManager>();
                if (storageManager != null)
                {
                    if (heldObj == null && storageManager.isFull)
                    {
                        storageManager.TryResetStorage();
                    }
                    else if (heldObj != null && heldObj.CompareTag("Glass") && !storageManager.isFull)
                    {
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
            else if (hit.collider.CompareTag("FoodBin"))
            {
                FoodBin foodBin = hit.collider.GetComponent<FoodBin>();
                if (foodBin != null && heldObj != null)
                {
                    Plate plate = heldObj.GetComponent<Plate>();
                    if (plate != null)
                    {
                        foodBin.StartScraping(plate);
                    }
                }
            }
            else if (hit.collider.CompareTag("LiquidBucket"))
            {
                LiquidBucket liquidBucket = hit.collider.GetComponent<LiquidBucket>();
                if (liquidBucket != null && heldObj != null)
                {
                    Glass glass = heldObj.GetComponent<Glass>();
                    if (glass != null)
                    {
                        liquidBucket.StartCleaning(glass);
                    }
                }
            }
            else if (hit.collider.CompareTag("TrashCan"))
            {
                TrashCan trashCan = hit.collider.GetComponent<TrashCan>();
                if (trashCan != null && heldObj != null)
                {
                    trashCan.StartDisposal(heldObj, this);
                }
            }
            else if (hit.collider.CompareTag("OrderDropZone"))
            {
                OrderDropZone orderDropZone = hit.collider.GetComponent<OrderDropZone>();
                if (orderDropZone != null && heldObj != null)
                {
                    // For non-tray orders, you might handle drop differently.
                    orderDropZone.HandleOrderDrop(heldObj.GetComponent<Collider>());
                }
            }

            else if (hit.collider.CompareTag("Tray"))
            {
                TrayManager trayManager = hit.collider.GetComponent<TrayManager>();

                if (trayManager != null && heldObj != null)
                {
                    Ticket ticket = heldObj.GetComponent<Ticket>();

                    if (ticket != null)
                    {
                        trayManager.PositionDocket(heldObj);
                    }
                    else if (heldObj.CompareTag("Order"))
                    {
                        trayManager.AddOrderToTray(heldObj);
                    }
                }
            }

            else if (hit.collider.CompareTag("Docket"))
            {
                DocketManager docketManager = hit.collider.GetComponent<DocketManager>();
                if (docketManager != null) // ? Prevent multiple dockets at once
                {
                    docketManager.ToggleDocketPosition();
                }
            }

        }
    }




    void PickUpObject(GameObject pickUpObj)
    {
        heldObj = pickUpObj;
        Rigidbody heldObjRb = heldObj.GetComponent<Rigidbody>();

        if (heldObjRb != null)
        {
            heldObjRb.isKinematic = true;
            heldObjRb.velocity = Vector3.zero; // Reset any movement
        }

        // Check if the object is a tray and apply custom hold position
        if (heldObj.CompareTag("Tray") || heldObj.CompareTag("OldPC"))
        {
            // Parent to tray hold position and apply correct rotation
            heldObj.transform.SetParent(trayHoldPos);
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.Euler(-90, 0, 0);  // Custom rotation for tray
        }
        else
        {
            // Parent other objects to the default hold position
            heldObj.transform.SetParent(holdPos);
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.identity;  // Default rotation
        }
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
                            return;
                        }
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
                            return;
                        }
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
                TrayManager trayManager = heldObj.GetComponent<TrayManager>();
                if (order != null)
                {
                    UpdateHoverUI(false, true, true, "Place Order?");
                }
                else if(trayManager != null)
                {
                    UpdateHoverUI(false, true, true, "Place Tray?");
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

            else if (hitObject.CompareTag("TrayPosition"))
            {
                // Ensure heldObj is not null before accessing its components
                TrayManager trayManager = null;
                if (heldObj != null)
                {
                    trayManager = heldObj.GetComponent<TrayManager>();
                }
                

                // If the player's hand is empty and the TrayPosition has a tray
                if (heldObj == null && hit.transform.childCount > 0)
                {
                    UpdateHoverUI(true, false, true, "Pick Up Tray?");
                }
                // If the player is holding a tray and TrayPosition is empty
                else if (heldObj != null)
                {
                    if (trayManager != null && hit.transform.childCount == 0)
                    {
                        UpdateHoverUI(false, true, true, "Place Tray?");
                    }
                    // If TrayPosition is full and player is holding a tray
                    else if (trayManager != null && hit.transform.childCount > 0)
                    {
                        UpdateHoverUI(false, true, true, "There's Already A Tray!");
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
}