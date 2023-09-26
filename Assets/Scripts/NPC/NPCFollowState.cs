using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollowState : WalkerBase
{
    public Transform targetTransform;
    public float distanceFromPlayer;

    public LookTowards lookTowards;

    public float minDist;

    public float currentDist;

    private void OnEnable()
    {
        Accelerate();
    }

    private void OnDisable()
    {
        Brake();
    }

    void FixedUpdate()
    {
        // Calculate the desired position behind the target.
        Vector3 desiredPosition = targetTransform.position - targetTransform.forward * distanceFromPlayer;

        // Calculate the direction to move towards the desired position.
        Vector3 moveDirection = (desiredPosition - rb.position).normalized;

        currentDist = Vector3.Distance(desiredPosition, rb.position);

        if (currentDist < minDist)
        {
            canMove = false;
            rb.velocity = Vector3.zero;
        }
        else
        {
            canMove = true;
        }

        movementVector2 = new Vector2(moveDirection.x, moveDirection.z);

        lookTowards.SetTarget(moveDirection);

        base.FixedUpdate();
    }
}
