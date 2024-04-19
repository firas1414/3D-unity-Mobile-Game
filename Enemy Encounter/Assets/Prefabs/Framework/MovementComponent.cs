using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] float turnSpeed = 8;
    public float RotationTowards(Vector3 AimDir)
    {
        float currentTrunSpeed = 0;
        if(AimDir.magnitude != 0)
        {
            Quaternion prevRot = transform.rotation;
            float turnlerpAlpha = turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(AimDir, Vector3.up),turnlerpAlpha);

            Quaternion currentRot = transform.rotation;
            float Dir = Vector3.Dot(AimDir, transform.right) >0 ? 1 : -1;
            float rotationDelta = Quaternion.Angle(prevRot, currentRot) * Dir;
            currentTrunSpeed = rotationDelta / Time.deltaTime;
        }
        return currentTrunSpeed;
    }
}
