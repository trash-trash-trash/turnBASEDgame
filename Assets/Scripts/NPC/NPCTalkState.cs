using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkState : MonoBehaviour
{
    //HACK TODO: FIX HACK
    //how to find this? player single ton? IPlayer?
    // Reference to the player's transform
    public Transform playerTransform;

    // Reference to the LookTowards component
    public LookTowards lookTowards;

    // Store the initial rotation when the component is enabled
    private Vector3 initialRotation;

    // Smoothing factor for rotation
    public float rotationSpeed = 2.0f;

    public void OnEnable()
    {
        // Store the initial rotation of the NPC
        initialRotation = lookTowards.targetPosition;
    }

    private void Update()
    {
        // Calculate the direction from the NPC to the player
        Vector3 directionToPlayer = playerTransform.position - lookTowards.transform.position;

        lookTowards.SetTarget(directionToPlayer);
    }

    public void OnDisable()
    {
        lookTowards.SetTarget(initialRotation);
    }
}