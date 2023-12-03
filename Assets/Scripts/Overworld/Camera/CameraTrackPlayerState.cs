using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayerState : MonoBehaviour
{
    public CameraBrain camBrain;

    public Transform playerTransform;

    public Transform camTransform;

    public float trackRotationSpeed;

    private float camMaxAngle;

    private bool trackingPlayer;

    private void OnEnable()
    {
        camMaxAngle = camBrain.maxAngle;
        playerTransform = PlayerBrain.Instance.transform;
        trackingPlayer = true;
    }

    void Update()
    {
        if (trackingPlayer)
        {
            // Calculate the direction from the camera to the player
            Vector3 directionToPlayer = playerTransform.position - camTransform.position;

            // Exclude the x and z components
            directionToPlayer.y = 0f;
            directionToPlayer.Normalize();

            // Calculate the limited rotation
            Quaternion limitedRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            // Limit the rotation if the angle exceeds the camMaxAngle
            float angleToPlayer = Vector3.Angle(camTransform.forward, directionToPlayer);
            if (angleToPlayer > camMaxAngle)
            {
                // Stay at the limited rotation, only rotating along the y-axis
                Vector3 limitedEulerAngles = limitedRotation.eulerAngles;
                limitedEulerAngles.x = 0f;
                limitedEulerAngles.z = 0f;
                limitedRotation = Quaternion.Euler(limitedEulerAngles);

                camTransform.rotation = Quaternion.RotateTowards(camTransform.rotation, limitedRotation,
                    Time.deltaTime * trackRotationSpeed);
            }
            else
            {
                // Smoothly rotate towards the player
                camTransform.rotation =
                    Quaternion.Slerp(camTransform.rotation, limitedRotation, Time.deltaTime * trackRotationSpeed);
            }
        }
    }
}