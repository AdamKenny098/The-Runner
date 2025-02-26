using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class OrderTaker : MonoBehaviour
{
    // When true, orders can be taken.
    public bool canTakeOrder = true;

    // Internal timer for the session
    private float orderTimer = 0f;

    [SerializeField] private float orderLeastWait;
    [SerializeField] private float orderMostWait;

    //Time between orders
    public float timeBetweenOrders;

    public int outcome;
    public TicketMaker orderCreator;
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenOrders = Random.Range(orderLeastWait, orderMostWait);
    }

    // Update is called once per frame
    void Update()
    {
        // Only increment session timer if not paused
        if (canTakeOrder)
        {
            orderTimer += Time.deltaTime;

            // Check if the session timer has reached the 20-minute mark
            if (orderTimer >= timeBetweenOrders)
            {
                outcome = Random.Range(1, 101);
                if (outcome % 2 == 0)
                {
                    orderCreator.CreateNewTicket();
                }
                else
                {
                    return;
                }
                orderTimer = 0f;
            }
        }
    }
}