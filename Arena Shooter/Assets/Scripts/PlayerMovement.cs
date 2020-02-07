using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float aerialMultiplier = 0.5f;
    public float jumpStrength = 10.0f;
    public float mouseSensitivity = 100.0f;

    CharacterController characterController;
    Vector3 motion = Vector3.zero;
    float xRotation = 0.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        CameraControl();
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            // Moving on the ground
            motion = transform.right * Input.GetAxis("Horizontal") * speed;
            motion += transform.forward * Input.GetAxis("Vertical") * speed;

            // Jumping
            if (Input.GetButton("Jump"))
            {
                motion.y = jumpStrength;
            }
        }
        else
        {
            // Moving in the air
            float ySpeed = motion.y;
            motion = transform.right * Input.GetAxis("Horizontal") * speed * aerialMultiplier;
            motion += transform.forward * Input.GetAxis("Vertical") * speed * aerialMultiplier;
            motion.y = ySpeed;
        }

        // Gravity
        motion.y += Physics.gravity.y * Time.fixedDeltaTime;

        // Final movement vector
        characterController.Move(motion * Time.fixedDeltaTime);
    }

    private void CameraControl()
    {
        // Getting values for horizontal and vertical mouse moves
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Horizontal movement
        gameObject.transform.Rotate(0, mouseX, 0);
    }
}
