using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateNextState : PartyControllerStateBase
{

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Hack());
    }

    private IEnumerator Hack()
    {
        yield return new WaitForFixedUpdate();

        if (ID == TurnTakerID.PlayerTwo)
            yield return new WaitForSeconds(AIController.logicWaitTime);

        Confirm();
    }

    protected override void Confirm()
    {
        base.Confirm();
        TurnTaker newTurnTaker = partyController.CalculateNextTurnTaker();

        if (newTurnTaker != null)
            partyController.ChangeState(PartyController.PartyControllerEnum.SelectPartyMember);

        else
        {
            partyController.ChangeState(PartyController.PartyControllerEnum.WaitingForNextTurn);
        }
    }

    protected override void Cancel()
    {
        base.Cancel();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectTarget);
    }
}