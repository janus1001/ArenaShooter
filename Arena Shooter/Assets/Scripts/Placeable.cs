using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public Renderer currentRenderer;

    public bool IsOnSurface;

    private const float blinkFrequency = 5;
    private const float baseTransparencyValue = 0.6f;
    private const float transparencyChangeStrength = 0.1f;

    public Vector3 groundedOffset;
    public Vector3 heldOffset;

    public float placementSpaceNeeded = 0.5f;
    public float minAngle = Mathf.NegativeInfinity;
    public float maxAngle = Mathf.Infinity;

    bool IsPlaceable
    {
        get
        {
            if (!IsOnSurface)
            {
                return false;
            }
            float rotationAngle = Vector3.Angle(transform.up, Vector3.up);
            if (rotationAngle > maxAngle || rotationAngle < minAngle)
            {
                return false;
            }
            if (Physics.OverlapSphere(transform.position, placementSpaceNeeded).Length > 1) // If there are more than one collider in radius, unable to place
            {
                return false;
            }
            return true;
        }
    }

    void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        Color color = IsPlaceable ? Color.cyan : Color.red;

        color.a = baseTransparencyValue + Mathf.Sin(Time.time * blinkFrequency) * transparencyChangeStrength;

        currentRenderer.material.color = color;
    }

    public void MoveToCursor(Vector3 position, Vector3 normalHit)
    {
        transform.position = position;
        transform.up = normalHit;

        if (IsOnSurface)
            transform.Translate(groundedOffset, Space.Self);
        else
            transform.Translate(heldOffset, Space.World);
    }
}
