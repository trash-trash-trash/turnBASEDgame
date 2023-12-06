using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Overworld.Camera;
using UnityEngine;

public class CameraBrain : MonoBehaviour
{
    public GameObjectStateManager stateManager;

    public CameraDetectPlayerBox detectBox;
    
    public GameObject camIdleState;
    public GameObject camReturnToIdleState;

    public GameObject camMoveToTrackRotationState;
    public GameObject camTrackPlayerState;

    public bool trackingPlayer = false;

    public float camMoveIdleSpeed;
    
    public float camWaitTime;
    public float maxAngle;
    
    public Quaternion originalRotation;

    private void OnEnable()
    {
        originalRotation = transform.rotation;
        detectBox.DeclarePlayerDetectedEvent += ChangeCamState;
        ChangeState(camIdleState);
    }

    private void ChangeCamState(bool input)
    {
        if(input)
            ChangeState(camMoveToTrackRotationState);
        else
            ChangeState(camReturnToIdleState);

        trackingPlayer = input;
    }

    public void ChangeState(GameObject newState)
    {
        stateManager.ChangeState(newState);
    }

    private void OnDisable()
    {
        detectBox.DeclarePlayerDetectedEvent -= ChangeCamState;
    }
}
