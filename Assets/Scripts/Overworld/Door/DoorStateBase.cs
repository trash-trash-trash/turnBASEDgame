using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStateBase : MonoBehaviour
{
    public DoorBrain brain;

    public Transform hingeTransform;

    public virtual void OnEnable()
    {
        brain = GetComponentInParent<DoorBrain>();
    }
}
