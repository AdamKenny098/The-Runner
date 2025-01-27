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
                // Raycast to detect objects to pick up
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
                {
                    if (hit.collider.CompareTag("canPickUp"))
                    {
                        PickUpObject(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                DropObject();
            }
        }

        // Separate key for interactions
        if (Input.GetKeyDown(KeyCode.E) && heldObj != null)
        {
            // Raycast to detect interactable objects
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
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
            {
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
        heldObj.transform.position = holdPos.position;
        heldObj.transform.SetParent(holdPos);
        Debug.Log($"Picked up object: {heldObj.name}");
    }

    void DropObject()
    {
        if (heldObj != null)
        {
            heldObjRb.isKinematic = false;
            heldObj.transform.SetParent(null);
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
                // Show the Hover UI and update text
                hoverUI.SetActive(true);
                hoverText.text = "Pick Up ?";
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
