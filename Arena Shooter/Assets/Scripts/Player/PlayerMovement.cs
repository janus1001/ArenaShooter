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
    Vector3 groundNormal = Vector3.up;
    bool isSlipping = false;
    float xRotation = 0.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            CameraControl();
            Move();
            Debug.DrawRay(transform.position, motion);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Terrain") || hit.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            groundNormal = hit.normal;
        }
        else
        {
            groundNormal = Vector3.up;
        }
    }

    private bool OnSlipperySlope()
    {
        if (characterController.isGrounded)
        {
            float groundAngle = Vector3.Angle(groundNormal, Vector3.up);

            if (groundAngle > characterController.slopeLimit)
            {
                return true;
            }
            else if (groundAngle != 0 && groundAngle <= characterController.slopeLimit)
            {
                motion.y = -slopeForce * Time.deltaTime;
                return false;
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
            if (motion.sqrMagnitude > motion.normalized.sqrMagnitude)
            {
                motion = motion.normalized;
            }
            motion *= speed;
        }
        else
        {
            // Moving in the air
            float ySpeed = motion.y;
            motion = transform.right * Input.GetAxis("Horizontal");
            motion += transform.forward * Input.GetAxis("Vertical");
            if (motion.sqrMagnitude > motion.normalized.sqrMagnitude)
            {
                motion = motion.normalized;
            }
            motion *= speed * aerialMultiplier;
            motion.y = ySpeed;
        }

        // Slope force
        if (isSlipping = OnSlipperySlope())
        {
            Vector3 groundCross = Vector3.Cross(groundNormal, Vector3.up);
            Vector3 slopeVector = Vector3.Cross(groundNormal, groundCross);
            motion = slopeVector * slopeForce * Time.deltaTime;
        }

        // Jumping
        if (characterController.isGrounded && !isSlipping && Input.GetButton("Jump"))
        {
            motion.y = jumpStrength;
        }

        // Gravity
        motion.y += Physics.gravity.y * Time.deltaTime;

        // Final movement vector
        characterController.Move(motion * Time.deltaTime);
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
