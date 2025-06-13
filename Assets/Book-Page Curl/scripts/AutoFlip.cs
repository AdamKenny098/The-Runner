// Refactored AutoFlip.cs to support hybrid flipping (visual animation + UI page content)

using UnityEngine;

[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour
{
    public Book ControledBook;
    public bool AutoStartFlip = true;

    private void Start()
    {
        if (!ControledBook)
            ControledBook = GetComponent<Book>();

        if (AutoStartFlip)
            FlipRightPage();
    }

    private void Update()
    {

        if (!ControledBook || !ControledBook.interactable)
        {
            Debug.Log("AutoFlip blocked (no book or not interactable)");
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("➡ Right Arrow Pressed");
            FlipRightPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FlipLeftPage();
        }
    }

    public void FlipRightPage()
    {
        if (ControledBook.currentPage < ControledBook.TotalPageCount - 1)
        {
            ControledBook.FlipRight();
        }
    }

    public void FlipLeftPage()
    {
        if (ControledBook.currentPage > 0)
        {
            ControledBook.FlipLeft();
        }
    }
}
