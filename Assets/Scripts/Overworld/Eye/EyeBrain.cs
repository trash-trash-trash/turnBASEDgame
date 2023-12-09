using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Overworld.Camera;
using UnityEngine;

public class EyeBrain : MonoBehaviour
{
    public CameraDetectPlayerBox detectBox;

    public GameObjectStateManager stateManager;

    public GameObject idleState;

    public GameObject returnIdleState;

    public GameObject playerDetectedState;

    public EyeView pupil;

    public Vector3 originalPupilPosition;

    public bool playerDetected;

    //add states driven events for multiple anims
    //EyeStates
    public event Action<bool> AnnouncePlayerDetected;

    void OnEnable()
    {
        //set for returnToIdleState
        originalPupilPosition = pupil.transform.position;
        
        detectBox.DeclarePlayerDetectedEvent += ChangePlayerDetected;
        
        ChangePlayerDetected(false);
    }

    public void ChangePlayerDetected(bool input)
    {
        playerDetected = input;
        AnnouncePlayerDetected?.Invoke(playerDetected);

        GameObject selectState = playerDetected ? playerDetectedState :  returnIdleState;
        ChangeState(selectState);
    }

    public void ChangeState(GameObject input)
    {
        stateManager.ChangeState(input);
    }

    void OnDisable()
    {
        detectBox.DeclarePlayerDetectedEvent -= ChangePlayerDetected;
    }
}