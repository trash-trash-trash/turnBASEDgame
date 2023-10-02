using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectActionState : PartyControllerStateBase
{
    public ActionsSingleton actionsSingleton;

    protected override void OnEnable()
    {
        base.OnEnable();

        partyController.DeclareActionOnOff(true);

        maxSelectInt = 4;
        selectInt = 0;

        ChangeSelectInt(0);
    }

    protected override void ChangeSelectInt(int x)
    {
        base.ChangeSelectInt(x);
        SelectAction(selectInt);
    }

    public void SelectAction(int x)
    {
        actionsSingleton = ActionsSingleton.Instance;

        partyController.currentAction = actionsSingleton.actionsDict[x];

        partyController.DeclareActionSelect(x);


        if (ID == TurnTakerID.PlayerOne)
            partyController.ChangeText(partyController.selectedPartyMember.name + " wants to use " + partyController.currentAction + "...", ID);
    }

    protected override void Confirm()
    {
        base.Confirm();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectTarget);
    }

    protected override void Cancel()
    {
        base.Cancel();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectPartyMember);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        partyController.DeclareActionOnOff(false);
    }
}
