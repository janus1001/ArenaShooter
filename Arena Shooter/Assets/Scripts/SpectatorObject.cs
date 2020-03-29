using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorObject : MonoBehaviour
{
    public static SpectatorObject singleton;

    [SerializeField]
    Behaviour[] disableComponentsWhenInactive;

    Transform mainCameraTransform;

    float xRotation = 0;
    float yRotation = 0;

    bool isActive;

    private void Start()
    {
        singleton = this;
    }

    void Update()
    {
        CameraControl();
        if (Camera.main)
        {
            Transform copyTransform = Camera.main.transform;

            if (copyTransform.position == Vector3.zero)
                return;

            transform.position = copyTransform.position;
            xRotation = copyTransform.rotation.x;
            yRotation = copyTransform.rotation.y;
        }
    }

    private void CameraControl()
    {
        // Getting values for horizontal and vertical mouse moves
        float mouseX = Input.GetAxis("Mouse X") * Settings.mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Settings.mouseSensitivity;

        // Vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
    }

    public static void SetSpectatorActive(bool state)
    {
        singleton.isActive = state;
        foreach (Behaviour comp in singleton.disableComponentsWhenInactive)
        {
            comp.enabled = state;
        }
    }
}
