//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 recoilPosition = Vector3.zero;
    private Vector3 recoilRotation = Vector3.zero;

    // Force describe how much the motion is accelerating in a given direction
    private Vector3 recoilPositionForce = Vector3.zero;
    private Vector3 recoilRotationForce = Vector3.zero;

    float positionStabilizationRate = 0.2f;
    float rotationStabilizationRate = 0.3f;

    float positionForceFallRate = 0.5f;
    float rotationForceFallRate = 0.3f;

    [Space(10)]
    // These are actual values for the weapons recoil
    public float recoilPositionSides = 0.3f;
    public float recoilPositionUp = 0.3f;
    public float recoilPositionForward = -1f;

    public float recoilRotationKnockup = -24;
    public float recoilRotationSlip = 0.2f;
    public float recoilRotationTwist = 0.1f;


    void Update()
    {
        recoilPosition = Vector3.Lerp(recoilPosition, recoilPositionForce, positionStabilizationRate);
        recoilRotation = Vector3.Lerp(recoilRotation, recoilRotationForce, rotationStabilizationRate);

        recoilPositionForce = Vector3.Lerp(recoilPositionForce, Vector3.zero, positionForceFallRate);
        recoilRotationForce = Vector3.Lerp(recoilRotationForce, Vector3.zero, rotationForceFallRate);

        transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPosition, positionStabilizationRate);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(recoilRotation), rotationStabilizationRate);
    }

    public void Punch()
    {
        recoilPositionForce += new Vector3(Random.Range(-recoilPositionSides, recoilPositionSides), Random.Range(recoilPositionUp * 0.8f, recoilPositionUp), Random.Range(recoilPositionForward * 0.8f, recoilPositionForward));
        recoilRotationForce += new Vector3(Random.Range(recoilRotationKnockup * 0.5f, recoilRotationKnockup), Random.Range(-recoilRotationSlip, recoilRotationSlip), Random.Range(-recoilRotationTwist, recoilRotationTwist));
    }
}
