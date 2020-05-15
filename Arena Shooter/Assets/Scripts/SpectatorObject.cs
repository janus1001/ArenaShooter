using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorObject : MonoBehaviour
{
    public static SpectatorObject singleton;
    public bool isSpectating = true;

    [SerializeField]
    Behaviour[] disableComponentsWhenInactive;

    Transform mainCameraTransform;

    float xRotation = 0;
    float yRotation = 0;
    private float normalMoveSpeed = 10f;
    private float climbSpeed = 10f;

    private void Start()
    {
        singleton = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(isSpectating)
        {
            if(Mirror.NetworkClient.connection.identity)
            {
                isSpectating = false;
            }
        }

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
        //Rotation
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

        if (isSpectating) //Movement
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime; 

            if (Input.GetKey(KeyCode.Space)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.LeftShift)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }
        }
    }

    public static void SetSpectatorActive(bool state)
    {
        foreach (Behaviour comp in singleton.disableComponentsWhenInactive)
        {
            comp.enabled = state;
        }
    }

    public static void SetIsSpectating(bool state)
    {
        singleton.isSpectating = state;
    }
}
