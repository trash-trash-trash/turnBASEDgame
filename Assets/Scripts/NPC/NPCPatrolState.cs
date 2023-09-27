using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class NPCPatrolState : MonoBehaviour
{
    public OverworldPatrol patrol;

    public LookTowards look;

    public Vector3 prevVector;

    public WalkerBase walker;

    public void OnEnable()
    {
        patrol.MovementVector3Event += ChangeVector;
    }

    private void ChangeVector(Vector3 newVector)
    {
        if (newVector != prevVector)
        {
            prevVector = newVector.normalized;
            look.SetTarget(prevVector);
        }
    }

    public void OnDisable()
    {
        walker.Brake();
        patrol.MovementVector3Event -= ChangeVector;
    }
}
