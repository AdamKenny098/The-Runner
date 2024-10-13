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

    [SerializeField]
    RectTransform BookPanel;

    public GameObject background;
    public GameObject[] bookPages; // Now holds Page Panels instead of Sprites
    public bool interactable = true;
    public bool enableShadowEffect = true;

        // Inside your Book class
    public Vector3 EndBottomLeft
    {
        get { return ebl; }
    }

    public Vector3 EndBottomRight
    {
        get { return ebr; }
    }


    // Represents the index of the current page (right page)
    public int currentPage = 0;

    public int TotalPageCount
    {
        get { return bookPages.Length; }
    }

    // Clipping and Shadow Images
    public Image ClippingPlane;
    public Image NextPageClip;
    public Image Shadow;
    public Image ShadowLTR;

    // Page Panels instead of Images
    public GameObject LeftPage;      // Replaces 'Left' Image
    public GameObject LeftNextPage;  // Replaces 'LeftNext' Image
    public GameObject RightPage;     // Replaces 'Right' Image
    public GameObject RightNextPage; // Replaces 'RightNext' Image

    public UnityEvent OnFlip;

    // Internal variables for page flipping calculations
    float radius1, radius2;
    Vector3 sb; // Spine Bottom
    Vector3 st; // Spine Top
    Vector3 c;  // Corner of the page
    Vector3 ebr; // Edge Bottom Right
    Vector3 ebl; // Edge Bottom Left
    Vector3 f;   // Follow point 
    bool pageDragging = false;
    FlipMode mode;

    void Start()
    {
        // Ensure the Book is a child of a Canvas
        if (!canvas) canvas = GetComponentInParent<Canvas>();
        if (!canvas) Debug.LogError("Book should be a child to a Canvas");

        // Initialize page panels
        LeftPage.SetActive(false);
        RightPage.SetActive(false);
        UpdatePages();
        CalcCurlCriticalPoints();

        // Configure clipping and shadow sizes based on BookPanel dimensions
        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        NextPageClip.rectTransform.sizeDelta = new Vector2(pageWidth, pageHeight + pageHeight * 2);

        ClippingPlane.rectTransform.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);

        // Calculate shadow dimensions
        float hyp = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
        float shadowPageHeight = pageWidth / 2 + hyp;

        Shadow.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        Shadow.rectTransform.pivot = new Vector2(1, (pageWidth / 2) / shadowPageHeight);

        ShadowLTR.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        ShadowLTR.rectTransform.pivot = new Vector2(0, (pageWidth / 2) / shadowPageHeight);
    }

    // Calculate critical points for page curling effect
    private void CalcCurlCriticalPoints()
    {
        sb = new Vector3(0, -BookPanel.rect.height / 2);
        ebr = new Vector3(BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        ebl = new Vector3(-BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        st = new Vector3(0, BookPanel.rect.height / 2);
        radius1 = Vector2.Distance(sb, ebr);
        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        radius2 = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
    }

    // Convert mouse screen position to local BookPanel position
    public Vector3 TransformPoint(Vector3 mouseScreenPos)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 mouseWorldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, canvas.planeDistance));
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseWorldPos);
            return localPos;
        }
        else if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 globalEBR = transform.TransformPoint(ebr);
            Vector3 globalEBL = transform.TransformPoint(ebl);
            Vector3 globalSt = transform.TransformPoint(st);
            Plane p = new Plane(globalEBR, globalEBL, globalSt);
            float distance;
            p.Raycast(ray, out distance);
            Vector2 localPos = BookPanel.InverseTransformPoint(ray.GetPoint(distance));
            return localPos;
        }
        else
        {
            // Screen Space Overlay
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseScreenPos);
            return localPos;
        }
    }

    void Update()
    {
        if (pageDragging && interactable)
        {
            UpdateBook();
        }
    }

    // Update the book's appearance based on mouse position
    public void UpdateBook()
    {
        f = Vector3.Lerp(f, TransformPoint(Input.mousePosition), Time.deltaTime * 10);
        if (mode == FlipMode.RightToLeft)
            UpdateBookRTLToPoint(f);
        else
            UpdateBookLTRToPoint(f);
    }

    // Update book for Left to Right flipping
    public void UpdateBookLTRToPoint(Vector3 followLocation)
    {
        mode = FlipMode.LeftToRight;
        f = followLocation;

        // Setup Shadow and ClippingPlane
        ShadowLTR.transform.SetParent(ClippingPlane.transform, true);
        ShadowLTR.transform.localPosition = Vector3.zero;
        ShadowLTR.transform.localEulerAngles = Vector3.zero;
        LeftPage.transform.SetParent(ClippingPlane.transform, true);

        RightPage.transform.SetParent(BookPanel.transform, true);
        RightPage.transform.localEulerAngles = Vector3.zero;
        LeftNextPage.transform.SetParent(BookPanel.transform, true);

        // Calculate page curl position
        c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(c, ebl, out t1);
        clipAngle = (clipAngle + 180) % 180;

        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
        ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

        // Position and rotate LeftPage
        LeftPage.transform.position = BookPanel.TransformPoint(c);
        float C_T1_dy = t1.y - c.y;
        float C_T1_dx = t1.x - c.x;
        float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
        LeftPage.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - 90 - clipAngle);

        // Setup NextPageClip
        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
        NextPageClip.transform.position = BookPanel.TransformPoint(t1);
        LeftNextPage.transform.SetParent(NextPageClip.transform, true);
        RightPage.transform.SetParent(ClippingPlane.transform, true);
        RightPage.transform.SetAsFirstSibling();

        ShadowLTR.rectTransform.SetParent(LeftPage.transform, true);
    }

    // Update book for Right to Left flipping
    public void UpdateBookRTLToPoint(Vector3 followLocation)
    {
        mode = FlipMode.RightToLeft;
        f = followLocation;

        // Setup Shadow and ClippingPlane
        Shadow.transform.SetParent(ClippingPlane.transform, true);
        Shadow.transform.localPosition = Vector3.zero;
        Shadow.transform.localEulerAngles = Vector3.zero;
        RightPage.transform.SetParent(ClippingPlane.transform, true);

        LeftPage.transform.SetParent(BookPanel.transform, true);
        LeftPage.transform.localEulerAngles = Vector3.zero;
        RightNextPage.transform.SetParent(BookPanel.transform, true);

        // Calculate page curl position
        c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(c, ebr, out t1);
        if (clipAngle > -90) clipAngle += 180;

        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);
        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

        // Position and rotate RightPage
        RightPage.transform.position = BookPanel.TransformPoint(c);
        float C_T1_dy = t1.y - c.y;
        float C_T1_dx = t1.x - c.x;
        float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
        RightPage.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - (clipAngle + 90));

        // Setup NextPageClip
        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        NextPageClip.transform.position = BookPanel.TransformPoint(t1);
        RightNextPage.transform.SetParent(NextPageClip.transform, true);
        LeftPage.transform.SetParent(ClippingPlane.transform, true);
        LeftPage.transform.SetAsFirstSibling();

        Shadow.rectTransform.SetParent(RightPage.transform, true);
    }

    // Calculate the clipping angle and position for page curl
    private float CalcClipAngle(Vector3 c, Vector3 bookCorner, out Vector3 t1)
    {
        Vector3 t0 = (c + bookCorner) / 2;
        float T0_CORNER_dy = bookCorner.y - t0.y;
        float T0_CORNER_dx = bookCorner.x - t0.x;
        float T0_CORNER_Angle = Mathf.Atan2(T0_CORNER_dy, T0_CORNER_dx);
        float T0_T1_Angle = 90 - T0_CORNER_Angle;

        float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
        T1_X = NormalizeT1X(T1_X, bookCorner, sb);
        t1 = new Vector3(T1_X, sb.y, 0);

        // Clipping plane angle = T0_T1_Angle
        float T0_T1_dy = t1.y - t0.y;
        float T0_T1_dx = t1.x - t0.x;
        T0_T1_Angle = Mathf.Atan2(T0_T1_dy, T0_T1_dx) * Mathf.Rad2Deg;
        return T0_T1_Angle;
    }

    // Normalize T1_X based on book corner and spine bottom
    private float NormalizeT1X(float t1X, Vector3 corner, Vector3 sb)
    {
        if (t1X > sb.x && sb.x > corner.x)
            return sb.x;
        if (t1X < sb.x && sb.x < corner.x)
            return sb.x;
        return t1X;
    }

    // Calculate the position 'c' based on follow location
    private Vector3 Calc_C_Position(Vector3 followLocation)
    {
        Vector3 c;
        f = followLocation;
        float F_SB_dy = f.y - sb.y;
        float F_SB_dx = f.x - sb.x;
        float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dx);
        Vector3 r1 = new Vector3(radius1 * Mathf.Cos(F_SB_Angle), radius1 * Mathf.Sin(F_SB_Angle), 0) + sb;

        float F_SB_distance = Vector2.Distance(f, sb);
        if (F_SB_distance < radius1)
            c = f;
        else
            c = r1;

        float F_ST_dy = c.y - st.y;
        float F_ST_dx = c.x - st.x;
        float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dx);
        Vector3 r2 = new Vector3(radius2 * Mathf.Cos(F_ST_Angle),
           radius2 * Mathf.Sin(F_ST_Angle), 0) + st;
        float C_ST_distance = Vector2.Distance(c, st);
        if (C_ST_distance > radius2)
            c = r2;
        return c;
    }

    // Handle dragging the right page to flip forward
    public void DragRightPageToPoint(Vector3 point)
    {
        if (currentPage >= bookPages.Length) return;
        pageDragging = true;
        mode = FlipMode.RightToLeft;
        f = point;

        // Adjust pivot points for flipping
        NextPageClip.rectTransform.pivot = new Vector2(0, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

        // Activate and setup LeftPage
        LeftPage.SetActive(true);
        LeftPage.transform.position = RightNextPage.transform.position;
        LeftPage.transform.eulerAngles = Vector3.zero;
        LeftPage.transform.SetAsFirstSibling();

        // Assign the appropriate page panel to LeftPage
        AssignPageToPanel(LeftPage, currentPage);

        // Activate and setup RightPage
        RightPage.SetActive(true);
        RightPage.transform.position = RightNextPage.transform.position;
        RightPage.transform.eulerAngles = Vector3.zero;
        AssignPageToPanel(RightPage, currentPage + 1);

        // Setup LeftNextPage
        AssignPageToPanel(LeftNextPage, currentPage + 2);

        if (enableShadowEffect) Shadow.gameObject.SetActive(true);
        UpdateBookRTLToPoint(f);
    }

    // Handle mouse drag event for right page
    public void OnMouseDragRightPage()
    {
        if (interactable)
            DragRightPageToPoint(TransformPoint(Input.mousePosition));
    }

    // Handle dragging the left page to flip backward
    public void DragLeftPageToPoint(Vector3 point)
    {
        if (currentPage <= 0) return;
        pageDragging = true;
        mode = FlipMode.LeftToRight;
        f = point;

        // Adjust pivot points for flipping
        NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(0, 0.35f);

        // Activate and setup RightPage
        RightPage.SetActive(true);
        RightPage.transform.position = LeftNextPage.transform.position;
        RightPage.transform.eulerAngles = Vector3.zero;
        AssignPageToPanel(RightPage, currentPage - 1);

        // Activate and setup LeftPage
        LeftPage.SetActive(true);
        LeftPage.transform.position = LeftNextPage.transform.position;
        LeftPage.transform.eulerAngles = Vector3.zero;
        AssignPageToPanel(LeftPage, currentPage - 2);

        // Setup RightNextPage
        AssignPageToPanel(RightNextPage, currentPage - 3);

        if (enableShadowEffect) ShadowLTR.gameObject.SetActive(true);
        UpdateBookLTRToPoint(f);
    }

    // Handle mouse drag event for left page
    public void OnMouseDragLeftPage()
    {
        if (interactable)
            DragLeftPageToPoint(TransformPoint(Input.mousePosition));
    }

    // Handle mouse release event
    public void OnMouseRelease()
    {
        if (interactable)
            ReleasePage();
    }

    // Determine whether to flip forward or back based on drag distance
    public void ReleasePage()
    {
        if (pageDragging)
        {
            pageDragging = false;
            float distanceToLeft = Vector2.Distance(c, ebl);
            float distanceToRight = Vector2.Distance(c, ebr);
            if (distanceToRight < distanceToLeft && mode == FlipMode.RightToLeft)
                TweenBack();
            else if (distanceToRight > distanceToLeft && mode == FlipMode.LeftToRight)
                TweenBack();
            else
                TweenForward();
        }
    }

    Coroutine currentCoroutine;

    // Assign a specific page panel to a given panel GameObject
    void AssignPageToPanel(GameObject panel, int pageIndex)
    {
        // Deactivate all child panels
        foreach (Transform child in panel.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (pageIndex >= 0 && pageIndex < bookPages.Length)
        {
            // Activate the specified page and set it as a child of the panel
            bookPages[pageIndex].SetActive(true);
            bookPages[pageIndex].transform.SetParent(panel.transform, true);
        }
        else
        {
            // If out of range, set the background
            background.SetActive(true);
            background.transform.SetParent(panel.transform, true);
        }
    }

    // Update pages initially
    void UpdatePages()
    {
        // Initially deactivate all pages except the first two
        for (int i = 0; i < bookPages.Length; i++)
        {
            if (i == 0 || i == 1)
                bookPages[i].SetActive(true);
            else
                bookPages[i].SetActive(false);
        }

        // Assign first two pages to LeftPage and RightPage
        if (bookPages.Length > 0)
        {
            AssignPageToPanel(LeftPage, 0);
        }
        if (bookPages.Length > 1)
        {
            AssignPageToPanel(RightPage, 1);
        }
    }

    // Tween the book to flip forward
    public void TweenForward()
    {
        if (mode == FlipMode.RightToLeft)
            currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f, () => { FlipForward(); }));
        else
            currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f, () => { FlipForward(); }));
    }

    // Finalize flipping forward
    void FlipForward()
    {
        if (mode == FlipMode.RightToLeft)
            currentPage += 2;
        else
            currentPage -= 2;

        // Reset parenting and deactivate flipping pages
        LeftNextPage.transform.SetParent(BookPanel.transform, true);
        LeftPage.transform.SetParent(BookPanel.transform, true);
        LeftNextPage.transform.SetParent(BookPanel.transform, true);
        LeftPage.SetActive(false);
        RightPage.SetActive(false);
        RightPage.transform.SetParent(BookPanel.transform, true);
        RightNextPage.transform.SetParent(BookPanel.transform, true);
        UpdatePages();
        Shadow.gameObject.SetActive(false);
        ShadowLTR.gameObject.SetActive(false);

        // Invoke flip event
        if (OnFlip != null)
            OnFlip.Invoke();
    }

    // Tween the book to flip back
    public void TweenBack()
    {
        if (mode == FlipMode.RightToLeft)
        {
            currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f,
                () =>
                {
                    UpdatePages();
                    RightNextPage.transform.SetParent(BookPanel.transform);
                    RightPage.transform.SetParent(BookPanel.transform);

                    LeftPage.SetActive(false);
                    RightPage.SetActive(false);
                    pageDragging = false;
                }
                ));
        }
        else
        {
            currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f,
                () =>
                {
                    UpdatePages();

                    LeftNextPage.transform.SetParent(BookPanel.transform);
                    LeftPage.transform.SetParent(BookPanel.transform);

                    LeftPage.SetActive(false);
                    RightPage.SetActive(false);
                    pageDragging = false;
                }
                ));
        }
    }

    // Coroutine to handle smooth flipping animation
    public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish)
    {
        int steps = (int)(duration / 0.025f);
        Vector3 displacement = (to - f) / steps;
        for (int i = 0; i < steps - 1; i++)
        {
            if (mode == FlipMode.RightToLeft)
                UpdateBookRTLToPoint(f + displacement);
            else
                UpdateBookLTRToPoint(f + displacement);

            yield return new WaitForSeconds(0.025f);
        }
        if (onFinish != null)
            onFinish();
    }
}
