using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTimer : MonoBehaviour
{
    public float maxDeliveryTime = 30f; // 30 seconds to deliver
    private float deliveryStartTime;

    private void Start()
    {
        deliveryStartTime = Time.time; // Start the timer when order is created
    }

    public bool IsLate()
    {
        return Time.time - deliveryStartTime > maxDeliveryTime;
    }
}
