using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public Renderer currentRenderer;

    public bool IsPlaceable { get; private set; }

    const float blinkFrequency = 5;

    void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        Color color = IsPlaceable ? Color.green : Color.red;

        color.a = 1.4f + (Mathf.Sin(Time.time * 5));

        currentRenderer.material.color = color;
    }
}
