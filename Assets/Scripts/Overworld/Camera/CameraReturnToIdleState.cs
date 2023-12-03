using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReturnToIdleState : MonoBehaviour
{
    public CameraBrain camBrain;

    public Transform camTransform;

    private IEnumerator returnToIdleRot;

    private float camSpinSpeed;

    private void OnEnable()
    {
        camSpinSpeed = camBrain.camMoveSpeed;
        returnToIdleRot = ReturnToIdleRot();
        StartCoroutine(returnToIdleRot);
    }

    IEnumerator ReturnToIdleRot()
    {
        Quaternion originalRotation = camBrain.originalRotation;
        Quaternion currentRotation = camTransform.rotation;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * camSpinSpeed;
            camTransform.rotation = Quaternion.Slerp(currentRotation, originalRotation, t);
            yield return null;
        }

        yield return new WaitForSeconds(camBrain.camWaitTime);

        camTransform.rotation = originalRotation;
        camBrain.ChangeState(camBrain.camIdleState);
    }

    private void OnDisable()
    {
        StopCoroutine(returnToIdleRot);
    }
}