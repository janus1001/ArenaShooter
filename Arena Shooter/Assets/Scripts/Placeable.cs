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
    
    public Vector3 offset;
    
    void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        Color color = IsOnSurface ? Color.cyan : Color.red;

        color.a = baseTransparencyValue + Mathf.Sin(Time.time * blinkFrequency) * transparencyChangeStrength;

        currentRenderer.material.color = color;
    }

    public void MoveToCursor(Vector3 position, Vector3 normalHit)
    {
        transform.position = position;
        transform.up = normalHit;

        //if(IsOnSurface)
            transform.Translate(offset, Space.Self);
    }
}
