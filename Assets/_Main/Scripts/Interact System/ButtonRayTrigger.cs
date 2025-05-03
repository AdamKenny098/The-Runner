using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonRayTrigger : MonoBehaviour
{
    public float interactionRange = 5f;
    public KeyCode triggerKey = KeyCode.Return;
    public ClockOut clockOut;

    

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Check if the object hit has a Button component
            Button button = hit.collider.GetComponent<Button>();

            if (button != null && Input.GetKeyDown(triggerKey))
            {
                clockOut.LoadLevelEvaluator();
                Debug.Log("Button triggered by looking + pressing key.");
            }
        }
    }
}
