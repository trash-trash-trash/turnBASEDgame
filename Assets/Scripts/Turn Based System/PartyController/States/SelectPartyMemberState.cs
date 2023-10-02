using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPartyMemberState : PartyControllerStateBase
{

    protected override void OnEnable()
    {
        base.OnEnable();
        
        maxSelectInt = partyController.party.Count - 1;

        if (ID == TurnTakerID.PlayerTwo)
        {
            int randomIndex = Random.Range(0, party.Count);

            selectInt = randomIndex;
        }

        selectInt = 0;
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

    protected virtual void ChangeSelectedPartyMember(int x)
    {
        partyController.selectedPartyMember = party[x];
        partyController.selectedTurnTaker = turnTakers[x];

        if (partyController.selectedTurnTaker.TurnTaken())
        {
            x++;
            ChangeSelectedPartyMember(x);
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
    }
}
