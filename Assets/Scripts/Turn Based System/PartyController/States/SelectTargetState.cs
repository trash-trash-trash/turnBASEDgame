using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : PartyControllerStateBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        //is -1 needed? still need clarification on this
        maxSelectInt = partyController.enemyParty.Count -1;
        selectInt = 0;

        ChangeSelectInt(0);
    }

    protected override void ChangeSelectInt(int x)
    {
        base.ChangeSelectInt(x);
        SelectTarget(selectInt);
    }

    public void SelectTarget(int x)
    {
        partyController.selectedEnemyPartyMember = enemyParty[x];
        partyController.selectedEnemyTurnTaker = enemyTurnTakers[x];

        if (ID == TurnTakerID.PlayerOne)
        {
            partyController.selectedEnemyTurnTaker.SetHighlighted(true);

            foreach (TurnTaker tt in enemyTurnTakers)
            {
                if (tt != partyController.selectedEnemyTurnTaker)
                {
                    tt.SetHighlighted(false);
                }
            }

            partyController.ChangeText("Use " + partyController.currentAction + " on " + partyController.selectedEnemyPartyMember.name + "...", ID);
        }
    }

    protected override void Confirm()
    {
        base.Confirm();
        partyController.selectedTurnTaker.EndTurn();
        partyController.selectedTurnTaker.SetHighlighted(false);
        partyController.ChangeState(PartyController.PartyControllerEnum.Calculate);
    }

    protected override void Cancel()
    {
        base.Cancel();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectAction);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (ID == TurnTakerID.PlayerOne)
            partyController.selectedEnemyTurnTaker.SetHighlighted(false);
    }
}
