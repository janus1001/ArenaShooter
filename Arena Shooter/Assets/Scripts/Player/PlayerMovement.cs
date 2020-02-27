using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 6.0f;
    public float aerialMultiplier = 0.5f;
    public float jumpStrength = 10.0f;
    public float mouseSensitivity = 100.0f;
    public float slopeForce = 5.0f;
    public float slopeRayLengthMultiplier = 1.5f;

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
        if(isLocalPlayer)
        {
            Move();
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            CameraControl();
        }
    }

    private bool OnSlope()
    {
        if (!characterController.isGrounded)
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * slopeRayLengthMultiplier))
        {
            if (hit.normal !=  Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            // Moving on the ground
            motion = transform.right * Input.GetAxis("Horizontal");
            motion += transform.forward * Input.GetAxis("Vertical");
            motion = motion.normalized * speed;
        }
        else
        {
            // Moving in the air
            float ySpeed = motion.y;
            motion = transform.right * Input.GetAxis("Horizontal");
            motion += transform.forward * Input.GetAxis("Vertical");
            motion = motion.normalized * speed * aerialMultiplier;
            motion.y = ySpeed;
        }

        // Slope force
        if ((motion.x != 0 || motion.z != 0) && OnSlope())
        {
            motion.y += slopeForce * Time.fixedDeltaTime;
        }

        // Jumping
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            motion.y = jumpStrength;
        }

        // Gravity
        motion.y += Physics.gravity.y * Time.fixedDeltaTime;

        // Final movement vector
        characterController.Move(motion * Time.fixedDeltaTime);
    }

    private void CameraControl()
    {
        // Getting values for horizontal and vertical mouse moves
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Horizontal movement
        gameObject.transform.Rotate(0, mouseX, 0);
    }
}
