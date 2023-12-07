using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeReturnIdleState : MonoBehaviour
{
    public EyeBrain brain;

    public Transform pupilTransform;

    public float returnSpeed;

    private IEnumerator coRoReturnIdle;

    private Vector3 targetPosition;

    void OnEnable()
    {
        coRoReturnIdle = ReturnToIdle();
        targetPosition = brain.transform.position;
        StartCoroutine(coRoReturnIdle);
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForFixedUpdate();

        // Calculate the direction from the pupil to the target position
        Vector3 directionToTarget = (pupilTransform.position - targetPosition).normalized;

        // Move towards the clamped position using Lerp
        pupilTransform.position = Vector3.Lerp(
            pupilTransform.position,
            directionToTarget,
            returnSpeed * Time.fixedDeltaTime
        );

        brain.ChangeState(brain.idleState);
    }

    private void OnDisable()
    {
        StopCoroutine(coRoReturnIdle);
    }
}