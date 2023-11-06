using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class PlayerOverworldMovement : WalkerBase, IPlayer
{
    public PlayerControls controls;

    public LookTowards look;

    private void OnEnable()
    {
        controls = PlayerControls.PlayerControlsInstance;
        controls.MovementEvent += AimVector;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("NPC"), true);
    }

    private void AimVector(Vector2 vector2)
    {
        movementVector2 = vector2;
        Vector3 newVector = new Vector3(vector2.x, 0, vector2.y);
        look.SetTarget(newVector);
    }

    private void OnDisable()
    {
        controls.MovementEvent -= AimVector;
    }
}
