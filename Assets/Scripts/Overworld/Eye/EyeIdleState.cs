using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeIdleState : MonoBehaviour
{
    public Transform pupil;
    
    private Vector3 initialPosition;
    private void OnEnable()
    {
        initialPosition = pupil.transform.position;
        pupil.transform.position = initialPosition;
    }

}
