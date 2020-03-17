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
        if(!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }

        if(cameraTransform)
        {
            transform.rotation = cameraTransform.rotation;
        }
    }
}
