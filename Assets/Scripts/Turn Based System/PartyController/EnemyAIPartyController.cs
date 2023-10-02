using System.Collections.Generic;
using UnityEngine;

public class EnemyAIPartyController : MonoBehaviour
{
    public float logicWaitTime;

    public PartyController partyController;

    public void OnEnable()
    {
        partyController.DeclareCurrentActionEvent += AITurn;
    }

    private void AITurn(PartyController.PartyControllerEnum input)
    {
        if (input == PartyController.PartyControllerEnum.SelectAction)
        {
            partyController.ChangeText(partyController.selectedPartyMember.HiddenTurnDescription, TurnTakerID.PlayerTwo);
        }
    }
}