using TMPro;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public Transform trayHoldPos;   // Custom hold position for the tray
    public float pickUpRange = 5f;
    public GameObject heldObj; // Made public so other scripts can access it
    private Rigidbody heldObjRb; // Rigidbody of the held object
    public LayerMask canPickUpLayer; // Layer for objects that can be picked up

    public TrayManager trayManager;
    void Update()
    {
        //HandleHoverUI();

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

        // Automatically clear heldObj if it�s no longer parented to the hold position.
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

        //Handle the updated logic for picking up stacks
        else if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
        {
            StackManager stackManager = hit.collider.GetComponent<StackManager>();
            if (stackManager != null && stackManager.stackedPlates.Count > 0)
            {
                GameObject newStack = stackManager.CreateStackContainer();
                PickUpObject(newStack);
                return;
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

                        AudioManager.Instance.PlaySFX(AudioManager.Instance.smallSplashSound);
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
                        AudioManager.Instance.PlaySFX(AudioManager.Instance.scrapingSound);
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
                        AudioManager.Instance.PlaySFX(AudioManager.Instance.largeSplashSound);
                        liquidBucket.StartCleaning(glass);
                    }
                }
            }
            else if (hit.collider.CompareTag("TrashCan"))
            {
                TrashCan trashCan = hit.collider.GetComponent<TrashCan>();
                if (trashCan != null && heldObj != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.garbageSound);
                    trashCan.StartDisposal(heldObj, this);
                }
            }
            else if (hit.collider.CompareTag("OrderDropZone"))
            {
                OrderDropZone orderDropZone = hit.collider.GetComponent<OrderDropZone>();
                if (orderDropZone != null && heldObj != null)
                {
                    // FIRST: Check if it's deliverable
                    if (heldObj.CompareTag("Order")) // Or whatever your deliverable tag is
                    {
                        // 🧠 Trigger the tutorial BEFORE delivery happens
                        if (TutorialManager.Instance != null)
                        {
                            TutorialManager.Instance.TriggerTutorial(
                                "firstOrder",
                                "First Delivery!",
                                "Nice job! You've delivered your first order. Keep it up and watch those timers!",
                                null
                            );
                        }

                        // THEN process the order normally
                        orderDropZone.HandleOrderDrop(heldObj.GetComponent<Collider>());
                    }
                    else
                    {
                        Debug.LogWarning("Trying to deliver an invalid object: " + heldObj.name);
                    }
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
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.docketSound);
                    docketManager.ToggleDocketPosition();
                }
            }

            else if (hit.collider.CompareTag("PlateWashUp"))
            {
                CounterStackManager counterStackManager = hit.collider.GetComponent<CounterStackManager>();
                if (counterStackManager != null && heldObj != null)
                {
                    if (heldObj.CompareTag("Stack"))
                    {
                        counterStackManager.AcceptStack(heldObj);
                        heldObj = null; // Reset held object after placing it in the stack
                    }
                }
            }

            else if (hit.collider.CompareTag("Locker"))
            {
                Debug.Log("Interacting with Locker...");
                LockerPuzzle locker = hit.collider.GetComponent<LockerPuzzle>();
                if (locker != null)
                {
                    locker.ActivatePuzzle();
                }
            }

        }
    }




    void PickUpObject(GameObject pickUpObj)
    {
        heldObj = pickUpObj;
        Rigidbody heldObjRb = heldObj.GetComponent<Rigidbody>();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pickUpSound);

        if (heldObjRb != null)
        {
            heldObjRb.isKinematic = true;
            heldObjRb.velocity = Vector3.zero;
        }

        if (heldObj.CompareTag("Tray") || heldObj.CompareTag("OldPC"))
        {
            heldObj.transform.SetParent(trayHoldPos);
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            heldObj.transform.SetParent(holdPos);
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.identity;
        }

        // 🔥 Tutorial detection
        TutorialInteractionTrigger tutorial = pickUpObj.GetComponent<TutorialInteractionTrigger>();
        if (tutorial != null)
        {
            tutorial.Trigger();
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
}