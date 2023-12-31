using System;
using UnityEngine;

public class LookTowards : MonoBehaviour
{
    public Vector3 targetPosition;
    public float rotationSpeed;

    public event Action<Vector3> AnnounceVisionTargetEvent;

    public void SetTarget(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        AnnounceVisionTargetEvent?.Invoke(newTargetPosition);
    }

    private void Update()
    {
        // Convert the Vector2 target position to a Vector3 with a constant Y value.
        Vector3 target = new Vector3(targetPosition.x, transform.localPosition.y, targetPosition.z);

        // Calculate the direction to the target.
        Vector3 directionToTarget = target - transform.localPosition;

        // Check if the direction to the target is not almost zero (a small threshold).
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Calculate the rotation needed to look at the target position in local space.
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Smoothly interpolate the current rotation to the rotation needed.
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationToTarget, rotationSpeed * Time.deltaTime);
        }
    }
}