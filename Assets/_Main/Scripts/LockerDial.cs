using UnityEngine;
using TMPro;

public class LockerDial : MonoBehaviour
{
    public TMP_Text display;
    public int currentValue = 0;
    public LockerPuzzle lockerPuzzle;

    public void Increase()
    {
        currentValue = (currentValue + 1) % 10;
        UpdateDisplay();
        lockerPuzzle.CheckCode();
    }

    public void Decrease()
    {
        currentValue = (currentValue - 1 + 10) % 10;
        UpdateDisplay();
        lockerPuzzle.CheckCode();
    }

    void UpdateDisplay()
    {
        display.text = currentValue.ToString();
    }
}
