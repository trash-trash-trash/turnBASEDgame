using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : MonoBehaviour
{
    public LookTowards lookTowards;

    public void OnEnable()
    {
        lookTowards.SetTarget(transform.forward);
    }
}
