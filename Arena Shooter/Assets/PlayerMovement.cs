using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float aerialMultiplier = 0.5f;
    public float jumpStrength = 10.0f;
    public float mouseSensitivity = 3.0f;

    CharacterController characterController;
    Vector3 motion = Vector3.zero;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        Camera.main.transform.Rotate(mouseSensitivity * mouseY, 0, 0);
        //Camera.main.transform.localRotation = Quaternion.Euler(Mathf.Clamp(Camera.main.transform.rotation.eulerAngles.x, 0, 180), 0, 0);

        gameObject.transform.Rotate(0, mouseSensitivity * mouseX, 0);
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            motion = transform.right * Input.GetAxis("Horizontal") * speed;
            motion += transform.forward * Input.GetAxis("Vertical") * speed;

            if (Input.GetButton("Jump"))
            {
                motion.y = jumpStrength;
            }
        }
        else
        {
            float ySpeed = motion.y;
            motion = transform.right * Input.GetAxis("Horizontal") * speed * aerialMultiplier;
            motion += transform.forward * Input.GetAxis("Vertical") * speed * aerialMultiplier;
            motion.y = ySpeed;
        }

        motion.y += Physics.gravity.y * Time.fixedDeltaTime;

        characterController.Move(motion * Time.fixedDeltaTime);
    }
}
