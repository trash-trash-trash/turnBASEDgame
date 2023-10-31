using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenState : DoorStateBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        brain.open = true;
    }
}
