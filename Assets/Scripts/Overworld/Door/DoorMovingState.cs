using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovingState : DoorStateBase
{
    private Quaternion startRotation;

    public override void OnEnable()
    {
        base.OnEnable();
        brain.moving = true;
        StartCoroutine(MoveDoor());
    }

    private IEnumerator MoveDoor()
    {
        Quaternion startRotation = hingeTransform.rotation;
        Quaternion endRotation;

        if (!brain.open)
        {
            // Consider the original opening direction.
            if (brain.openedPositively)
            {
                endRotation = startRotation * Quaternion.Euler(0, brain.swingAmount, 0);
            }
            else
            {
                endRotation = startRotation * Quaternion.Euler(0, -brain.swingAmount, 0);
            }
        }
        else
        {
            endRotation = brain.doorClosedRotation;
        }

        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < brain.swingTime)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / brain.swingTime);

            hingeTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        hingeTransform.rotation = endRotation; // Ensure the final rotation is exact.

        brain.moving = false;

        if (brain.open)
            brain.ChangeState(brain.closedState);
        else
            brain.ChangeState(brain.openState);
    }
}