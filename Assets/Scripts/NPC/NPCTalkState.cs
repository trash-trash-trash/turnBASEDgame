using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkState : MonoBehaviour
{
    //HACK TODO: FIX HACK
    //how to find this? player single ton? IPlayer?
    public Transform playerTransform;

    public LookTowards lookTowards;

    public Vector3 prevVector;

    public NPCBrain brain;

    public void OnEnable()
    {
        prevVector = lookTowards.targetPosition;
        lookTowards.SetTarget(playerTransform.position.normalized);
    }


    public void OnDisable()
    {
        lookTowards.SetTarget(prevVector);
    }
}