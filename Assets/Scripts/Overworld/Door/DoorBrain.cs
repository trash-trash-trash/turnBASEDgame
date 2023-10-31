using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class DoorBrain : MonoBehaviour
{
    public GameObjectStateManager stateManager;

    public enum DoorStates
    {
        Open,
        Closed,
        Moving,
        Locked
    }

    public DoorStates currentState;

    public bool locked;
    public bool open;
    public bool moving=false;

    public GameObject openState;
    public GameObject closedState;
    public GameObject movingState;
    public GameObject lockedState;

    public Transform playerTransform;

    public Quaternion doorClosedRotation;
    
    public Dictionary<DoorStates, GameObject> statesDict;

    public bool isOnPositiveSide;

    public float swingAmount;

    public float swingTime;

    public bool opening;

    public bool openedPositively;

    public DoorInteractBox side1;
    public DoorInteractBox side2;

    void Start()
    {
        playerTransform = PlayerBrain.Instance.transform;

        side1.DeclareDoorInteractEvent += OpenClose;
        side2.DeclareDoorInteractEvent += OpenClose;
        
        statesDict = new Dictionary<DoorStates, GameObject>
        {
            { DoorStates.Open, openState },
            { DoorStates.Closed, closedState },
            {DoorStates.Moving, movingState },
            { DoorStates.Locked, lockedState }
        };
        
        if(locked)
            ChangeState(lockedState);
        
        else if(open)
            ChangeState(openState);
        
        else
            ChangeState(closedState);
    }

    public void OpenClose(DoorInteractBox.DoorSide newDoorSide)
    {
        if (moving)
            return;

        if (newDoorSide == DoorInteractBox.DoorSide.Side1)
            openedPositively = true;

        else
            openedPositively = false;

        GameObject newState = null;

        switch (currentState)
        {
            case DoorStates.Closed:
                opening = true;
                newState = movingState;
                break;

            case DoorStates.Open:
                opening = false;
                newState = movingState;
                break;

            case DoorStates.Locked:
                break;
        }

        if (newState != null)
        {
            ChangeState(newState);
        }
    }

    public void ChangeState(GameObject newState)
    {
        DoorStates newStateEnum = DoorStates.Locked;
        foreach (var entry in statesDict)
        {
            if (entry.Value == newState)
            {
                newStateEnum = entry.Key;
                break;
            }
        }

        currentState = newStateEnum;

        stateManager.ChangeState(newState);
    }

    void OnDisable()
    {
        side1.DeclareDoorInteractEvent -= OpenClose;
        side2.DeclareDoorInteractEvent -= OpenClose;
    }
}
