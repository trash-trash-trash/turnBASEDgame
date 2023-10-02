using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectActionState : PartyControllerStateBase
{
    public ActionsSingleton actionsSingleton;

    protected override void OnEnable()
    {
        base.OnEnable();

        if(ID==TurnTakerID.PlayerOne)
         partyController.DeclareActionOnOff(true);

        maxSelectInt = 4;
        selectInt = 0;

        StartCoroutine(Hack());
    }

    private IEnumerator Hack()
    {
        yield return new WaitForFixedUpdate();

        if (ID == TurnTakerID.PlayerTwo)
            yield return new WaitForSeconds(AIController.logicWaitTime);

        ChangeSelectInt(selectInt);
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

        //figure out custom AI behaviour later
        if (ID == TurnTakerID.PlayerTwo)
        {
            partyController.ChangeText(partyController.selectedPartyMember.ContemplateTurnDescription, ID);
            Confirm();
        }
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

        if(ID==TurnTakerID.PlayerOne)
            partyController.DeclareActionOnOff(false);
    }
}
