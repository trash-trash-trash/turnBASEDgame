using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIdleState : MonoBehaviour
{
    public Transform cameraTransform;
    public CameraBrain camBrain;

    private float camSpinSpeed;
    private float camWaitTime;

    private Quaternion originalRotation;
    private IEnumerator spinCamera;

    void OnEnable()
    {
        originalRotation = Quaternion.Euler(0, 180, 0);
        cameraTransform.rotation = originalRotation;
        camWaitTime = camBrain.camWaitTime;
        camSpinSpeed = camBrain.camMoveIdleSpeed;
        spinCamera = SpinCamera();
        StartCoroutine(spinCamera);
    }

    IEnumerator SpinCamera()
    {
        while (true)
        {
            // Rotate to (0, camBrain.maxAngle, 0)
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, camBrain.maxAngle, 0);
            yield return RotateCameraSmoothly(targetRotation);

            // Wait for camWaitTime seconds
            yield return new WaitForSeconds(camWaitTime);

            // Rotate to (0, -camBrain.maxAngle, 0)
            targetRotation = originalRotation * Quaternion.Euler(0, -camBrain.maxAngle, 0);
            yield return RotateCameraSmoothly(targetRotation);

            // Wait for camWaitTime seconds
            yield return new WaitForSeconds(camWaitTime);
        }
    }

    IEnumerator RotateCameraSmoothly(Quaternion targetRotation)
    {
        float t = 0;
        Quaternion startRotation = cameraTransform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * camSpinSpeed;
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(spinCamera);
    }
}