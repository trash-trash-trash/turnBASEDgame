using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;

public class NPCBrain : MonoBehaviour
{
    public List<PartyMemberScriptableObject> party;

    public NPCTypeEnum myType;

    public GameObjectStateManager stateManager;

    public PartyInventory partyInventory;

    public GameObject attackObj;

    public GameObject chaseObj;

    public GameObject returnHomeObj;

    public GameObject fightObj;

    public GameObject followObj;

    public GameObject idleObj;

    public GameObject patrolObj;

    public GameObject talkObj;

    public GameObject deathObj;

    public Dictionary<OverworldNPCStates, GameObject> stateDictionary = new Dictionary<OverworldNPCStates, GameObject>();

    public Talker talker;

    public VisionBase vision;

    public FightStarter fighter;

    public bool isAlive=true;

    public event Action<OverworldNPCStates> AnnounceStateEvent;

    public OverworldNPCStates currentState;

    private void OnEnable()
    {
        if (myType == NPCTypeEnum.Enemy)
        {
            party = partyInventory.party;
            stateDictionary.Add(OverworldNPCStates.Attack, attackObj);
            stateDictionary.Add(OverworldNPCStates.Chase, chaseObj);
            stateDictionary.Add(OverworldNPCStates.ReturnHome, returnHomeObj);
            stateDictionary.Add(OverworldNPCStates.Fight, fightObj);
            stateDictionary.Add(OverworldNPCStates.Death, deathObj);  
            vision.SeePlayerBoolEvent += ChaseState;
            fighter.lookingToFight = true;
            fighter.FightStartedEvent += FightState;
            vision.SeePlayerBoolEvent += AttackState;
        }
        
        stateDictionary.Add(OverworldNPCStates.Follow, followObj);
        stateDictionary.Add(OverworldNPCStates.Idle, idleObj);
        stateDictionary.Add(OverworldNPCStates.Patrol, patrolObj);
        stateDictionary.Add(OverworldNPCStates.Talk, talkObj);


        talker.OpenDialogueEvent += TalkState;

        talker.CloseDialogueEvent += PatrolState;

        if (myType == NPCTypeEnum.Stationary)
            ChangeState(OverworldNPCStates.Idle);

        else
            ChangeState(OverworldNPCStates.Patrol);
    }

    private void ChaseState(bool input)
    {
        if(input)
            ChangeState(OverworldNPCStates.Chase);
    }   

    private void OnDisable()
    {
        if (myType == NPCTypeEnum.Enemy)
        {
            fighter.FightStartedEvent -= FightState;
            vision.SeePlayerBoolEvent -= AttackState;
        }

        talker.OpenDialogueEvent -= TalkState;

        talker.CloseDialogueEvent -= PatrolState;
    }

    public void ChangeTestState()
    {
        ChangeState(currentState);
    }

    private void AttackState(bool input)
    {
        if(currentState==OverworldNPCStates.Patrol)
        if(input)
            ChangeState(OverworldNPCStates.Attack);
    }

    private void TalkState()
    {
        ChangeState(OverworldNPCStates.Talk);
    }

    private void FightState()
    {
        ChangeState(OverworldNPCStates.Fight);
    }

    private void PatrolState()
    {
        if (isAlive)
        {
            if (myType == NPCTypeEnum.Enemy)
            {
                ChangeState(OverworldNPCStates.Fight);
            }

            else if (myType == NPCTypeEnum.Stationary)
                ChangeState(OverworldNPCStates.Idle);

            else
                ChangeState(OverworldNPCStates.Patrol);
        }
        else
        {
            ChangeState(OverworldNPCStates.Death);
        }
    }

    private void Death(StatsEnum statsEnum, int i)
    {
        if (statsEnum != StatsEnum.HP)
            return;
        if (i <= 0)
        {
            ChangeState(OverworldNPCStates.Death);
        }
    }

    public void ChangeState(OverworldNPCStates inputState)
    {
        if (stateDictionary.TryGetValue(inputState, out GameObject stateObject))
        {
            stateManager.ChangeState(stateObject);
            AnnounceStateEvent?.Invoke(inputState);

        }
        else
        {
            //think state goes here
            Debug.LogError("State not found: " + inputState);
        }
    }
    
}