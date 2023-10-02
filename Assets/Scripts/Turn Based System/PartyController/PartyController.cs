using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor.EditorTools;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    public TurnTakerID myID;

    public GameObjectStateManager stateManager;

    public GameObject selectPartyMemberObj;
    public GameObject selectActionObj;
    public GameObject selectTargetObj;
    public GameObject calculateStateObj;
    public GameObject waitForNextTurnObj;

    public PartyControllerEnum currentState;

    public Dictionary<PartyControllerEnum, GameObject> statesDictionary =
        new Dictionary<PartyControllerEnum, GameObject>();

    public bool myTurn;

    public PartyMemberScriptableObject selectedPartyMember;
    public PartyMemberScriptableObject selectedEnemyPartyMember;

    public List<PartyMemberScriptableObject> party;
    public List<PartyMemberScriptableObject> enemyParty;

    public TurnTaker selectedTurnTaker;
    public List<TurnTaker> turnTakers;

    public TurnTaker selectedEnemyTurnTaker;
    public List<TurnTaker> enemyTurnTakers;

    public event Action<bool> ActionSelectEvent;

    public event Action<int> SelectActionIntEvent;

    public TurnController turnController;

    public ActionsSingleton.Actions currentAction;

    public float useActionWaitTime;

    //sub AI to events
    public DealerAI dealerAI;

    public ActionsSingleton actionsSingleton;

    public event Action<PartyControllerEnum> DeclareCurrentActionEvent;

    protected virtual void OnEnable()
    {
        turnController.SetPartyEvent += SetParty;

        statesDictionary.Add(PartyControllerEnum.SelectPartyMember, selectPartyMemberObj);
        statesDictionary.Add(PartyControllerEnum.SelectAction, selectActionObj);
        statesDictionary.Add(PartyControllerEnum.SelectTarget, selectTargetObj);
        statesDictionary.Add(PartyControllerEnum.Calculate, calculateStateObj);
        statesDictionary.Add(PartyControllerEnum.WaitingForNextTurn, waitForNextTurnObj);
    }

    public void DeclareActionSelect(int x)
    {
        SelectActionIntEvent?.Invoke(x);
    }

    public void DeclareActionOnOff(bool input)
    {
        ActionSelectEvent?.Invoke(input);
    }

    public enum PartyControllerEnum
    {
        SelectPartyMember,
        SelectAction,
        SelectTarget,
        WaitingForNextTurn,
        Calculate
    }

    public void ChangeState(PartyControllerEnum newState)
    {
        if (statesDictionary.ContainsKey(newState))
        {
            stateManager.ChangeState(statesDictionary[newState]);
            currentState = newState;
            DeclareCurrentActionEvent?.Invoke(currentState);
        }
    }

    public void ChangeAction(ActionsSingleton.Actions newAction)
    {
        currentAction = newAction;
    }

    protected virtual void OnDisable()
    {
        turnController.SetPartyEvent -= SetParty;
    }

    protected virtual void UseActionOnTarget()
    {
        StartCoroutine(ConfirmAction());
    }

    private IEnumerator ConfirmAction()
    {
        ChangeText(
            selectedPartyMember.name + " uses " + selectedPartyMember.selectedAction.actionName + " on " +
            selectedEnemyPartyMember.name, myID);

        yield return new WaitForSeconds(useActionWaitTime);

        dealerAI.AddAttack(selectedPartyMember, selectedEnemyPartyMember, selectedPartyMember.selectedAction);
    }


    private void SelectParty()
    {
        ActionSelectEvent?.Invoke(false);
    }

    public void SetParty(List<PartyMemberScriptableObject> newParty, List<PartyMemberScriptableObject> newEnemyParty,
        List<TurnTaker> newTurnTakers, List<TurnTaker> newEnemyTurnTakers)
    {
        if (myID == TurnTakerID.PlayerOne)
        {
            party = newParty;
            enemyParty = newEnemyParty;
            turnTakers = newTurnTakers;
            enemyTurnTakers = newEnemyTurnTakers;
        }

        else if (myID == TurnTakerID.PlayerTwo)
        {
            party = newEnemyParty;
            enemyParty = newParty;
            turnTakers = newEnemyTurnTakers;
            enemyTurnTakers = newTurnTakers;
        }

        foreach (TurnTaker tt in turnTakers)
        {
            tt.StartTurn();
        }

        StartTurn();
    }

    protected virtual void StartTurn()
    {
        ChangeState(PartyControllerEnum.SelectPartyMember);
    }

    public void ChangeText(string input, TurnTakerID ID)
    {
        turnController.SetControllerText(input, ID);
    }

    public TurnTaker CalculateNextTurnTaker()
    {
        TurnTaker newTurnTaker = null;

        foreach (TurnTaker tt in turnTakers)
        {
            if (!tt.TurnTaken())
            {
                newTurnTaker = tt;
                break;
            }
        }

        if (newTurnTaker != null)
        {
            return newTurnTaker;
        }

        return null;
    }
}