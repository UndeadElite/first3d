using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public Transform followPlayer;
    public Vector3 playeroffset;
    public float moveSpeed = 5;

    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (followPlayer != null)
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playeroffset, moveSpeed * Time.deltaTime); ;
    }
}
