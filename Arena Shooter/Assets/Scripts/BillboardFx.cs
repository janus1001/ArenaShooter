using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFx : MonoBehaviour
{
    Transform cameraTransform;
    //Quaternion originalRotation;

    private void Start()
    {
        //originalRotation = transform.rotation;
    }

    void Update()
    {
        if (!cameraTransform && Camera.main)
        {
            Transform newCameraTransform = Camera.main.transform;
            if (newCameraTransform)
            {
                cameraTransform = newCameraTransform;
            }
        }

        if (cameraTransform)
        {
            transform.rotation = cameraTransform.rotation;
        }
    }
}
