using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentManager : MonoBehaviour
{
    [SerializeField] private GameObject computerInApartment;
    [SerializeField] private GameObject brokenTVStand;
    [SerializeField] private GameObject brokenKitchen;
    [SerializeField] private GameObject fixedTVStand;
    [SerializeField] private GameObject fixedKitchen;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
            return;
        }

        if (GameManager.Instance.hasBoughtComputer)
        {
            computerInApartment.SetActive(true);
        }

        if(GameManager.Instance.hasRepairedTVStand)
        {
            brokenTVStand.SetActive(false);
            fixedTVStand.SetActive(true);
        }
        else
        {
            brokenTVStand.SetActive(true);
            fixedTVStand.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
