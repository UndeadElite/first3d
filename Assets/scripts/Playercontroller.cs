using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Guncontroller))]
public class Playercontroller : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Camera mainCamera;
    private float rayLength;
    private Guncontroller guncontroller;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        guncontroller = GetComponent<Guncontroller>(); // Initialize guncontroller
    }

    void FixedUpdate()
    {
        // Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(inputX, 0.0f, inputY).normalized * speed;

        // Set the velocity directly for immediate movement response
        rb.velocity = movement;

        // Face camera
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        // Weapon input
        if (Input.GetMouseButton(0))
        {
            guncontroller.Shoot();
        }
    }
}
