using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class PlayerOverworldMovement : WalkerBase
{
    public PlayerControls controls;

    private void OnEnable()
    {
        controls.MovementEvent += AimVector;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("NPC"), true);
    }

    private void AimVector(Vector2 vector2)
    {
        movementVector2 = vector2;
    }

    private void OnDisable()
    {
        controls.MovementEvent -= AimVector;
    }
}
