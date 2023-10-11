using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPartyMemberState : PartyControllerStateBase
{
    //TO DO: PUT A CHECK IF THERE IS ONLY ONE PARTY MEMBER LEFT TO CONFIRM AND INSTA CONFIRM IT
    protected override void OnEnable()
    {
        base.OnEnable();
        
        maxSelectInt = partyController.party.Count - 1;
        selectInt = 0;

        ChangeSelectInt(0);

        StartCoroutine(Hack());
    }

    private IEnumerator Hack()
    {
        yield return new WaitForFixedUpdate();
        ChangeSelectedPartyMember(selectInt);
    }

    protected override void Confirm()
    {
        base.Confirm();
        partyController.ChangeState(PartyController.PartyControllerEnum.SelectAction);
    }

    protected override void ChangeSelectInt(int amount)
    {
        base.ChangeSelectInt(amount);
        ChangeSelectedPartyMember(selectInt);
    }

    protected void HighlightNextUnfinishedTurnTaker(int startIndex)
    {
        for (int i = startIndex + 1; i < turnTakers.Count; i++)
        {
            if (!partyController.turnTakers[i].TurnTaken())
            {
                ChangeSelectedPartyMember(i);
                return;
            }
            else
            {
                Debug.Log("Out of turn takers");
            }
        }
    }

    protected virtual void ChangeSelectedPartyMember(int x)
    {
        partyController.selectedPartyMember = party[x];
        partyController.selectedTurnTaker = turnTakers[x];

        if (partyController.selectedTurnTaker.TurnTaken())
        {
            HighlightNextUnfinishedTurnTaker(x);
            return;
        }

        if (ID == TurnTakerID.PlayerOne)
        {
            partyController.selectedTurnTaker.SetHighlighted(true);

            foreach (TurnTaker tt in turnTakers)
            {
                if (tt != partyController.selectedTurnTaker)
                {
                    tt.SetHighlighted(false);
                }
            }

            partyController.ChangeText(partyController.selectedPartyMember.name, ID);
        }

        if (partyController.party.Count -1 <= 0)
            Confirm();

        if (ID == TurnTakerID.PlayerTwo)
            Confirm();
    }
}
