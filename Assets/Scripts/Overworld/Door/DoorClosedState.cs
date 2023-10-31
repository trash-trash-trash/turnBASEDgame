using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosedState : DoorStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        brain.open = false;

        brain.doorClosedRotation = hingeTransform.rotation;
    }
}
