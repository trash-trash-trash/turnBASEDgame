using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateNextState : PartyControllerStateBase
{
    protected override void OnEnable()
    {
        base.OnEnable(); 
        CheckTurnTakers(); // Call CheckTurnTakers when the state is enabled
    }

    private void CheckTurnTakers()
    {
        bool anyFalse = false; // Initialize anyFalse as false

        foreach (TurnTaker tt in turnTakers)
        {
            if (!tt.TurnTaken()) // Check if TurnTaken is false
            {
                anyFalse = true; // Set anyFalse to true when a false value is encountered
                break; // No need to continue checking, we found one false value
            }
        }

        if (anyFalse)
        {
            NextPartyMember();
        }
        else
        {
            WaitForNextTurn();
        }
    }

    private void NextPartyMember()
    {
       partyController.ChangeState(PartyController.PartyControllerEnum.SelectPartyMember);
       Debug.Log("still turns to be taken");
    }

    private void WaitForNextTurn()
    {
        if (ID == TurnTakerID.PlayerOne)
            partyController.selectedTurnTaker.SetHighlighted(false);
        
        partyController.myTurn = false;
       partyController.ChangeState(PartyController.PartyControllerEnum.WaitingForNextTurn);
       Debug.Log("all turns used");
    }

    protected override void Confirm()
    {
        base.Confirm();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectPartyMember);
    }

    protected override void Cancel()
    {
        base.Cancel();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectTarget);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}