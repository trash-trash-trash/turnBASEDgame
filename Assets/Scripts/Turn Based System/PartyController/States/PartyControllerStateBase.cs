using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyControllerStateBase : MonoBehaviour
{
    public PartyController partyController;

    public TurnController turnController;

    public PlayerControls playerControls;

    public List<PartyMemberScriptableObject> party;

    public List<TurnTaker> turnTakers;

    public List<PartyMemberScriptableObject> enemyParty;

    public List<TurnTaker> enemyTurnTakers;

    public TurnTakerID ID;

    public int selectInt;

    public int maxSelectInt;

    protected virtual void OnEnable()
    {
        ID = partyController.myID;

        party = partyController.party;
        turnTakers = partyController.turnTakers;

        enemyParty = partyController.enemyParty;
        enemyTurnTakers = partyController.enemyTurnTakers;

        if (ID == TurnTakerID.PlayerOne)
        {
            playerControls = PlayerControls.PlayerControlsInstance;
            playerControls.MenuConfirmEvent += Confirm;
            playerControls.MenuCancelEvent += Cancel;
            playerControls.MenuMovementEvent += MenuMovement;
        }
    }

    private void MenuMovement(Vector2 vector2)
    {
        if (vector2.x > 0 || vector2.y > 0)
        {
            ChangeSelectInt(-1);
        }
        else if (vector2.x < 0 || vector2.y < 0)
        {
            ChangeSelectInt(1);
        }
    }

    protected virtual void ChangeSelectInt(int amount)
    {
        int newInt = selectInt + amount;

        if (newInt < 0)
        {
            newInt = maxSelectInt;
        }

        else if (newInt > maxSelectInt)
        {
            newInt = 0;
        }

        selectInt = newInt;
    }

    protected virtual void Confirm()
    {
    }

    protected virtual void Cancel()
    {

    }

    protected virtual void NextState()
    {

    }

    protected virtual void OnDisable()
    {
        if (ID == TurnTakerID.PlayerOne)
        {
            playerControls.MenuConfirmEvent -= Confirm;
            playerControls.MenuCancelEvent -= Cancel;
            playerControls.MenuMovementEvent -= MenuMovement;
        }
    }
}