using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    public bool myTurn;
    public bool selectParty;
    public bool selectAction;
    public bool selectTarget;

    public int selectInt = 0;
    private int actionSelectInt = 0;
    private int partySelectInt = 0;

    public int maxSelectInt;

    public PartyMemberScriptableObject selectedPartyMember;
    public PartyMemberScriptableObject selectedEnemyPartyMember;

    public List<PartyMemberScriptableObject> party;
    public List<PartyMemberScriptableObject> enemyParty;

    private TurnTaker selectedTurnTaker;
    public List<TurnTaker> turnTakers;

    private TurnTaker selectedEnemyTurnTaker;
    public List<TurnTaker> enemyTurnTakers;

    public event Action<bool> ActionSelectEvent;
    public event Action<int> SelectIntEvent;

    public TurnController turnController;

    public enum Actions
    {
        Attack,
        Spells,
        Items,
        Guard,
        Flee
    }

    public Actions currentAction;

    public PlayerControls playerControls;

    public Dictionary<int, Actions> actionsDict = new Dictionary<int, Actions>();

    public void OnEnable()
    {
        actionsDict.Add(0, Actions.Attack);
        actionsDict.Add(1, Actions.Spells);
        actionsDict.Add(2, Actions.Items);
        actionsDict.Add(3, Actions.Guard);
        actionsDict.Add(4, Actions.Flee);

        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MenuMovementEvent += MenuMovement;
        playerControls.MenuConfirmEvent += MenuConfirm;
        playerControls.MenuCancelEvent += MenuCancel;
    }

    private void OnDisable()
    {
        playerControls.MenuMovementEvent -= MenuMovement;
        playerControls.MenuConfirmEvent -= MenuConfirm;
        playerControls.MenuCancelEvent -= MenuCancel;
    }

    private void MenuMovement(Vector2 vector2)
    {
        if (myTurn)
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
    }

    public void ChangeSelectInt(int amount)
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

        if (selectParty)
            ChangeSelectedPartyMember(selectInt);
        
        else if (selectAction)
            ChangeSelectAction(selectInt);

        else if (selectTarget)
            ChangeSelectedTarget(selectInt);

        SelectIntEvent?.Invoke(selectInt);
    }

    public void ChangeSelectAction(int x)
    {
        currentAction = actionsDict[x];
        ChangeText(selectedPartyMember.name + " wants to use " + currentAction + "...");
    }

    public void ChangeSelectedPartyMember(int x)
    {
        selectedPartyMember = party[x];
        selectedTurnTaker = turnTakers[x];

        if (selectedTurnTaker.turnTaken)
        {
            return;
        }

        selectedTurnTaker.SetHighlighted(true);

        foreach (TurnTaker tt in turnTakers)
        {
            if (tt != selectedTurnTaker)
            {
                tt.SetHighlighted(false);
            }
        }
        ChangeText(selectedPartyMember.name);
    }

    public void ChangeSelectedTarget(int x)
    {
        selectedEnemyPartyMember = enemyParty[x];
        selectedEnemyTurnTaker = enemyTurnTakers[x];
        selectedEnemyTurnTaker.SetHighlighted(true);

        foreach (TurnTaker tt in enemyTurnTakers)
        {
            if (tt != selectedEnemyTurnTaker)
            {
                tt.SetHighlighted(false);
            }
        }
        ChangeText("Use "+currentAction+" on "+selectedEnemyPartyMember.name+"...");
    }

    public void MenuConfirm()
    {
        if (myTurn)
        {
            if (selectParty)
            {
                SelectAction();
            }

            else if (selectAction)
            {
                SelectTarget();
            }
            else if (selectTarget)
            {
                UseActionOnTarget();
            }
        }
    }

    private void MenuCancel()
    {
        if (myTurn)
        {
            if (selectAction)
            {
                SelectParty();
            }
            else if (selectTarget)
            {
                SelectAction();
            }
        }
    }

    private void SelectAction()
    {
        ActionSelectEvent?.Invoke(true);
        selectInt = 0;
        maxSelectInt = 4;
        selectParty = false;
        selectAction = true;
        ChangeSelectInt(0);
    }

    private void SelectTarget()
    {
        ActionSelectEvent?.Invoke(false);
        selectInt = 0;
        maxSelectInt = enemyParty.Count - 1;
        selectTarget = true;
        selectAction = false;
        ChangeSelectInt(0);
    }

    private void UseActionOnTarget()
    {
        selectedTurnTaker.EndTurn();

        int newSelectInt = selectInt + 1;
        selectTarget = false;
        selectParty = true;

        selectedEnemyTurnTaker.SetHighlighted(false);

        bool allTrue = true; // Initialize allTrue as true

        foreach (TurnTaker tt in turnTakers)
        {
            if (!tt.TurnTaken()) // Check if TurnTaken is false
            {
                allTrue = false; // Set allTrue to false when a false value is encountered
                break;
            }
        }

        if (allTrue)
        {
            selectedTurnTaker.SetHighlighted(false);

            myTurn = false;
        }
        else
        {
            ChangeSelectInt(newSelectInt);
        }
    }

    private void SelectParty()
    {
        ActionSelectEvent?.Invoke(false);
        selectParty = true;
        selectAction = false;
        maxSelectInt = party.Count - 1;
        ChangeSelectInt(0);
    }

    public void SetParty(List<PartyMemberScriptableObject> newParty, List<PartyMemberScriptableObject> newEnemyParty,
        List<TurnTaker> newTurnTakers, List<TurnTaker> newEnemyTurnTakers)
    {
        party = newParty;
        enemyParty = newEnemyParty;
        turnTakers = newTurnTakers;
        enemyTurnTakers = newEnemyTurnTakers;

        myTurn = true;
        selectParty = true;
        selectTarget = false;

        maxSelectInt = party.Count - 1;

        ChangeSelectInt(0);
    }

    private void ChangeText(string input)
    {
        turnController.SetControllerText(input);
    }
}