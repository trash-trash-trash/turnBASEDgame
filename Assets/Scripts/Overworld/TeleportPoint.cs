using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public Transform playerTransform;

    public Transform teleportToPoint;

    public void SetTeleportTransform(Transform teleportPoint)
    {
        teleportToPoint = teleportPoint;
    }

    void OnTriggerEnter(Collider other)
    {
        playerTransform = other.transform;
    }

    public void Teleport()
    {
        if (playerTransform != null)
        {
            playerTransform.position = teleportToPoint.position;
        }
    }

    public void Interact()
    {
        Teleport();
    }
}
