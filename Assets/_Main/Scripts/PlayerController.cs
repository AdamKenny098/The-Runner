using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 2f;
    Vector3 forward;
    CharacterController controller;

    Camera fpsCam;
    Ray fpsRay;
    RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        fpsCam = GameObject.Find("1st Person Camera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,Input.GetAxis("Horizontal")*rotationSpeed,0);
        forward = transform.TransformDirection(0,0,1);
        float currentSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * currentSpeed);
    }
}
