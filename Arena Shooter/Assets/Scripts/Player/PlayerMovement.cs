using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 6.0f;
    public float aerialMultiplier = 0.5f;
    public float jumpStrength = 10.0f;
    public float slopeForce = 300.0f;

    CharacterController characterController;
    Vector3 positionLastFrame;
    Vector3 motion = Vector3.zero;
    Vector3 groundColision = Vector3.zero;
    bool isStill = true;
    bool isSlipping = false;
    float xRotation = 0.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        positionLastFrame = transform.position;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            isStill = transform.position == positionLastFrame;
            positionLastFrame = transform.position;
            if (!Settings.settingsInstance.gameObject.activeSelf)
            {
                CameraControl();
                Move();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Terrain") || hit.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            groundColision = hit.point;
            groundColision -= transform.position;
        }
    }

    private bool OnSlipperySlope()
    {
        Ray groundRay = new Ray(transform.position + groundColision + Vector3.up, Vector3.down);
        if (characterController.isGrounded && Physics.Raycast(groundRay, out RaycastHit groundHit))
        {
            float groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);

            if (groundAngle > characterController.slopeLimit)
            {
                Vector3 groundCross = Vector3.Cross(groundHit.normal, Vector3.up);
                Vector3 slopeVector = Vector3.Cross(groundHit.normal, groundCross); ;
                motion = slopeVector * slopeForce * Time.deltaTime;
                return true;
            }
            else if (groundAngle != 0 && groundAngle <= characterController.slopeLimit)
            {
                motion.y = -slopeForce * Time.deltaTime;
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
        if (isSlipping = OnSlipperySlope()) // this if must be executed just after OnSlipperySlope
        {
        }

        // Jumping
        if (characterController.isGrounded && !isSlipping && Input.GetButton("Jump") || isSlipping && isStill && Input.GetButton("Jump"))
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
        float mouseX = Input.GetAxis("Mouse X") * Settings.mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Settings.mouseSensitivity;

        // Vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Horizontal movement
        gameObject.transform.Rotate(0, mouseX, 0);
    }
}
