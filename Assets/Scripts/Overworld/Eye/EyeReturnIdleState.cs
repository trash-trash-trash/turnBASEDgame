using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyeReturnIdleState : MonoBehaviour
{
    public EyeBrain brain;
    public Transform pupilTransform;
    public float returnSpeed;
    public float minDist;

    private IEnumerator coRoReturnIdle;
    private Vector3 targetPosition;

    // Start the coroutine when the script is enabled
    void OnEnable()
    {
        coRoReturnIdle = ReturnToIdle();
        targetPosition = brain.originalPupilPosition;
        StartCoroutine(coRoReturnIdle);
    }

    // Coroutine to smoothly return the pupil to its idle state
    IEnumerator ReturnToIdle()
    {
        while (Vector3Distance(pupilTransform.position, targetPosition) > minDist)
        {
            yield return new WaitForFixedUpdate();

            // Calculate the direction from the pupil to the target position
            Vector3 directionToTarget = (targetPosition - pupilTransform.position).normalized;

            // Move towards the target position using Lerp
            pupilTransform.position += directionToTarget * returnSpeed * Time.fixedDeltaTime;
        }

        // Ensure the pupil is exactly at the target position when done
        pupilTransform.position = targetPosition;

        // Change the state using the EyeBrain
        brain.ChangeState(brain.idleState);
    }

    // Helper function to calculate the distance between two Vector3 points
    private float Vector3Distance(Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1, v2);
    }
    
    // Stop the coroutine when the script is disabled
    private void OnDisable()
    {
        StopCoroutine(coRoReturnIdle);
    }
}