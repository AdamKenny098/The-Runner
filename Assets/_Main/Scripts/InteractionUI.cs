using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI_Layout : MonoBehaviour
{
    [Header("Detection")]
    public Transform playerCamera;
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public PickUpSystem pickUpSystem;

    [Header("UI")]
    public GameObject uiPanel;
    public GameObject actionRowPrefab;
    public Transform rowContainer; // The parent object with Vertical Layout Group

    private List<GameObject> currentRows = new List<GameObject>();

    public void Start()
    {
        pickUpSystem = FindAnyObjectByType<PickUpSystem>();
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        GameObject heldObj = pickUpSystem.heldObj;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            string tag = hit.collider.tag;
            int layer = hit.collider.gameObject.layer;
            ShowUIForTag(tag, layer, heldObj, hit);
        }
        else
        {
            // New: If nothing is hit but something is held, show fallback UI
            if (heldObj != null)
            {
                ShowFallbackHeldUI(heldObj);
            }
            else
            {
                HideUI();
            }
        }


    }

    private void ShowUIForTag(string tag, int layer, GameObject heldObj, RaycastHit hit)
    {
        ClearRows();

        List<(string key, string action)> actions = new();

        heldObj = pickUpSystem.heldObj;

        switch (tag)
        {

            case "Plate":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }

            break;

            case "Glass":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }
            break;

            case "Order":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }
            break;

            case "Waste":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }
            break;

            case "Recycling":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }
            break;

            case "Stack":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }
            break;

            case "Tray":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("F", "Pick Up"));
            }
            else 
            {
            
            }
            break;

            case "Docket":
            if (layer == LayerMask.NameToLayer("CanPickUp") && heldObj == null)
            {
                actions.Add(("E", "Read"));
                actions.Add(("F", "Pick Up"));
            }
            break;

            case "FoodBin":
                if (tag == "FoodBin" && layer == LayerMask.NameToLayer("Default"))
                {
                    if (heldObj != null)
                    {
                        Plate plate = heldObj.GetComponent<Plate>();
                        if (plate != null && plate.IsDirty())
                        {
                            actions.Add(("E", "Scrape Plate"));
                            actions.Add(("F", "Drop"));
                        }
                    }
                }
            break;

            case "GlassStorage":

                GlassStorageManager glassStorage = hit.collider.GetComponent<GlassStorageManager>();

                // If the player's hand is empty and the storage is full
                if (heldObj == null && glassStorage.isFull)
                {
                    actions.Add(("E", "Process Glasses"));
                    actions.Add(("F", "Drop"));
                }
                // If the player is holding a glass and the storage is not full
                else if (heldObj != null)
                {
                    Glass glass = heldObj.GetComponent<Glass>();

                    if (glass != null && glassStorage.nextGlassIndex < glassStorage.glassSlots.Length)
                    {
                        actions.Add(("E", "Place Glass"));
                        actions.Add(("F", "Drop"));
                    }
                    // If storage is full and the player is holding a glass
                    else if (glass != null && glassStorage.nextGlassIndex == glassStorage.glassSlots.Length)
                    {
                        actions.Add(("F", "Drop"));
                    }
                }
                break;

            case "LiquidBucket":
                if (heldObj != null)
                {
                    Glass glass = heldObj.GetComponent<Glass>();
                    if (glass != null && glass.IsDirty())
                    {
                        actions.Add(("E", "Empty Glass"));
                        actions.Add(("F", "Drop"));
                    }
                }
                break;

            case "TrashCan":

                if (hit.collider.CompareTag("TrashCan") && heldObj != null)
                {
                    TrashCan trashCan = hit.collider.GetComponent<TrashCan>();
                    if (trashCan != null)
                    {
                        if (heldObj.CompareTag(trashCan.acceptedTag))
                        {
                            actions.Add(("E", "Dispose"));
                            actions.Add(("F", "Drop"));
                        }
                    }
                }
                if (heldObj != null)
                {
                    Plate plate = heldObj.GetComponent<Plate>();
                    if (plate != null && plate.IsDirty())
                    {
                        actions.Add(("E", "Scrape Plate"));
                    }
                }
                break;

            case "Untagged":
                if(heldObj != null)
                {
                    actions.Add(("F", "Drop"));
                }
                break;

            case "OrderDropZone":
                if (heldObj != null)
                {
                    Order order = heldObj.GetComponent<Order>();
                    TrayManager trayManager = heldObj.GetComponent<TrayManager>();
                    if (order != null)
                    {
                        actions.Add(("E", "Place Order"));
                        actions.Add(("F", "Drop"));
                    }
                    else if (trayManager != null)
                    {
                        actions.Add(("E", "Place Tray"));
                        actions.Add(("F", "Drop"));
                    }
                }
                break;

            case "TrayPosition":
                TrayManager trayManager1 = null;
                if (heldObj != null)
                {
                    trayManager1 = heldObj.GetComponent<TrayManager>();
                }


                // If the player's hand is empty and the TrayPosition has a tray
                if (heldObj == null && hit.transform.childCount > 0)
                {
                    actions.Add(("E", "Pick Up Tray"));
                }
                // If the player is holding a tray and TrayPosition is empty
                else if (heldObj != null)
                {
                    if (trayManager1 != null && hit.transform.childCount == 0)
                    {
                        actions.Add(("E", "Place Tray"));
                        actions.Add(("F", "Drop"));
                    }
                }
                break;

            //Apartment Scene
            case "FrontDoor":
                if (layer == LayerMask.NameToLayer("Default") && heldObj == null)
                {
                    actions.Add(("E", "Leave for Work"));
                }
                break;
            

            default:
                HideUI();
                return;
        }

        if (heldObj != null && actions.Count == 0)
        {
            actions.Add(("F", "Drop"));
        }

        if (actions.Count == 0)
        {
            HideUI();
            return;
        }

        foreach (var action in actions)
        {
            GameObject row = Instantiate(actionRowPrefab, rowContainer);
            row.transform.localScale = Vector3.one;

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0].text = action.key;
            texts[1].text = action.action;

            currentRows.Add(row);
        }

        uiPanel.SetActive(true);
    }

    private void ClearRows()
    {
        foreach (var row in currentRows)
        {
            Destroy(row);
        }
        currentRows.Clear();
    }

    private void ShowFallbackHeldUI(GameObject heldObj)
    {
        ClearRows();

        string label = "Drop";

        GameObject row = Instantiate(actionRowPrefab, rowContainer);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
        texts[0].text = "F";
        texts[1].text = label;
        currentRows.Add(row);

        uiPanel.SetActive(true);
    }


    private void HideUI()
    {
        ClearRows();
        uiPanel.SetActive(false);
    }
}
