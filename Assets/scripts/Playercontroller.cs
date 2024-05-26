using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float speed = 400f;
    private Rigidbody rb;
    private Camera MainCamera;
    float rayLength;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MainCamera = FindAnyObjectByType<Camera>();
    }

    
    void FixedUpdate()
    {
        //Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(inputX, 0.0f, inputY);
        rb.AddForce(movement);

        //Face camera
        Ray cameraray = MainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        if(groundPlane.Raycast(cameraray, out rayLength ))
        {
            Vector3 PointToLook = cameraray.GetPoint(rayLength);
            Debug.DrawLine(cameraray.origin,PointToLook,Color.yellow);

            transform.LookAt(new Vector3(PointToLook.x, transform.position.y, PointToLook.z));
        }

    }
}
