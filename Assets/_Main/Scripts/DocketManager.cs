using UnityEngine;
using System.Collections;

public class DocketManager : MonoBehaviour
{
    public Transform readingPosition; // The position in front of the player (child of the camera)
    public float moveSpeed = 5f; // Speed at which the docket moves

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    private bool isReading = false;

    private void Start()
    {
        originalPosition = Vector3.zero;
        originalRotation = Quaternion.Euler(0,0,0);
        originalParent = transform.parent; // Store original parent
        readingPosition = GameObject.Find("Docket Position").transform;
    }

    public void ToggleDocketPosition()
    {
        if (isReading)
        {
            ResetDocket();
        }
        else
        {
            ReadDocket();
        }
    }

    private void ReadDocket()
    {
        isReading = true;
        transform.SetParent(readingPosition); // ? Parent to camera so it moves with player
        StopAllCoroutines();
        StartCoroutine(MoveDocket(Vector3.zero, readingPosition.localRotation));
    }

    private void ResetDocket()
    {
        isReading = false;
        transform.SetParent(originalParent); // ? Restore original parent
        StopAllCoroutines();
        StartCoroutine(MoveDocket(originalPosition, originalRotation));
    }

    private IEnumerator MoveDocket(Vector3 targetPosition, Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (elapsedTime < 1f)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPosition, elapsedTime * moveSpeed);
            transform.localRotation = Quaternion.Lerp(startRot, targetRotation, elapsedTime * moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }
}
