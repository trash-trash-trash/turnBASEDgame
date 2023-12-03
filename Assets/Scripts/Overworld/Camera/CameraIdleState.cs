using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIdleState : MonoBehaviour
{
    public Transform cameraTransform;

    public CameraBrain camBrain;

    float camSpinSpeed;
    float camAngle;
    float camWaitTime;

    private Quaternion originalRotation;
    
    private IEnumerator spinCamera;

    void OnEnable()
    {
        originalRotation = camBrain.originalRotation;
        cameraTransform.rotation = originalRotation;
        camAngle = camBrain.maxAngle;
        camWaitTime = camBrain.camWaitTime;
        camSpinSpeed = camBrain.camMoveSpeed;
        spinCamera = SpinCamera();
        StartCoroutine(spinCamera);
    }

    IEnumerator SpinCamera()
    {
        while (true)
        {
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, camAngle, 0);

            // Rotate the camera smoothly towards the target rotation
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * camSpinSpeed;
                cameraTransform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t);
                yield return null;
            }

            yield return new WaitForSeconds(camWaitTime);
            
            // Return to the original rotation smoothly
            t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * camSpinSpeed;
                cameraTransform.rotation = Quaternion.Slerp(targetRotation, originalRotation, t);
                yield return null;
            }

            yield return new WaitForSeconds(camWaitTime); // Add a small delay before starting the next iteration
        }
    }

    private void OnDisable()
    {
        StopCoroutine(spinCamera);
    }
}
