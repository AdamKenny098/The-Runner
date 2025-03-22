using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI_Layout : MonoBehaviour
{
    [Header("Detection")]
    public Transform playerCamera;
    public float interactionRange = 3f;
    public LayerMask interactableLayer;

    [Header("UI")]
    public GameObject uiPanel;
    public GameObject actionRowPrefab;
    public Transform rowContainer; // The parent object with Vertical Layout Group

    private List<GameObject> currentRows = new List<GameObject>();

    private void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            string tag = hit.collider.tag;
            ShowUIForTag(tag);
        }
        else
        {
            HideUI();
        }
    }

    private void ShowUIForTag(string tag)
    {
        ClearRows();

        List<(string key, string action)> actions = new();

        switch (tag)
        {
            case "Plate":
                actions.Add(("G", "Drop"));
                break;

            case "Glass":
                actions.Add(("G", "Drop"));
                actions.Add(("R", "Throw"));
                break;

            case "Door":
                actions.Add(("Mouse", "Open"));
                break;

            case "Trash":
                actions.Add(("G", "Dispose"));
                break;

            default:
                HideUI();
                return;
        }

        foreach (var action in actions)
        {
            GameObject row = Instantiate(actionRowPrefab, rowContainer);
            row.transform.localScale = Vector3.one; // Ensure proper scaling

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

    private void HideUI()
    {
        ClearRows();
        uiPanel.SetActive(false);
    }
}
