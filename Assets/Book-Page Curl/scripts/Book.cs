// Hybrid Refactored Book.cs: Supports page-flip animation (visual) + UI prefab-based content

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public enum FlipMode
{
    RightToLeft,
    LeftToRight
}

[ExecuteInEditMode]
public class Book : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform BookPanel;

    [Tooltip("Assign UI prefabs here")]
    public GameObject[] bookPages; // UI prefab pages

    public int currentPage = 0;
    public bool interactable = true;

    public Transform LeftPageContainer;
    public Transform RightPageContainer;

    public Image LeftPageVisual;
    public Image RightPageVisual;

    public UnityEvent OnFlip;

    public int TotalPageCount => bookPages.Length;

    void Start()
    {
        if (!canvas) canvas = GetComponentInParent<Canvas>();
        if (!canvas) Debug.LogError("Book should be a child to canvas");

        UpdateUIPageContent();
    }

    public void FlipRight()
    {
        Debug.Log("📖 Book.FlipRight() called");
        Debug.Log($"Current page: {currentPage}, Total: {TotalPageCount}");

        if (currentPage + 2 < TotalPageCount)
        {
            StartCoroutine(FlipAnimation(true));
            Debug.Log($"📖 Flipping to page {currentPage + 2}");
        }
    }



    public void FlipLeft()
    {
        if (currentPage - 2 >= 0)
        {
            StartCoroutine(FlipAnimation(false));
        }
    }

    IEnumerator FlipAnimation(bool right)
    {
        Debug.Log($"📖 Starting flip animation to {(right ? "right" : "left")}");

        Image pageToFlip = right ? RightPageVisual : LeftPageVisual;
        // Optional: Show visual flip animation
        if (pageToFlip != null)
        {
            yield return StartCoroutine(AnimatePageFlip(pageToFlip, right));
        }
        else
        {
            // Fallback if no visual page is assigned
            yield return new WaitForSecondsRealtime(0.3f);
        }

        currentPage += right ? 2 : -2;
        UpdateUIPageContent();
        OnFlip?.Invoke();
        Debug.Log($"📖 Flip animation completed to page {currentPage}");
    }

    private void UpdateUIPageContent()
    {
        ClearContainer(LeftPageContainer);
        ClearContainer(RightPageContainer);

        if (currentPage >= 0 && currentPage < bookPages.Length)
        {
            Instantiate(bookPages[currentPage], LeftPageContainer);
        }

        if (currentPage + 1 < bookPages.Length)
        {
            Instantiate(bookPages[currentPage + 1], RightPageContainer);
        }

        Debug.Log($"📖 Flipped to pages {currentPage} & {currentPage + 1}");

    }

    private void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    public void GoToPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < bookPages.Length && pageIndex % 2 == 0)
        {
            currentPage = pageIndex;
            UpdateUIPageContent();
            OnFlip?.Invoke();
        }
    }
    
    IEnumerator AnimatePageFlip(Image pageVisual, bool right)
    {
        pageVisual.gameObject.SetActive(true);
        pageVisual.transform.localScale = new Vector3(0f, 1f, 1f); // Start closed

        float duration = 0.3f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / duration;

            float scale = right ? progress : 1 - progress;
            pageVisual.transform.localScale = new Vector3(scale, 1f, 1f);

            yield return null;
        }

        pageVisual.gameObject.SetActive(false);
    }

} 
