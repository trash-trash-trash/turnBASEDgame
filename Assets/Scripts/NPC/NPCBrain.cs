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

    public GameObject AttackObj;

    public GameObject FightObj;

    public GameObject FollowObj;

    public GameObject IdleObj;

    public GameObject PatrolObj;

    public GameObject TalkObj;

    public GameObject DeathObj;

    public Dictionary<NPCStates, GameObject> stateDictionary = new Dictionary<NPCStates, GameObject>();

    public Talker talker;

    public VisionBase vision;

    public FightStarter fighter;

    public bool isAlive=true;

    public enum NPCStates
    {
        Attack,
        Fight,
        Follow,
        Idle,
        Patrol,
        Talk,
        Death
    }

    public NPCStates currentState;

    private void OnEnable()
    {
        if(myType==NPCTypeEnum.Enemy)
         party = partyInventory.party;

        stateDictionary.Add(NPCStates.Attack, AttackObj);
        stateDictionary.Add(NPCStates.Fight, FightObj);
        stateDictionary.Add(NPCStates.Follow, FollowObj);
        stateDictionary.Add(NPCStates.Idle, IdleObj);
        stateDictionary.Add(NPCStates.Patrol, PatrolObj);
        stateDictionary.Add(NPCStates.Talk, TalkObj);
        stateDictionary.Add(NPCStates.Death, DeathObj);

        talker.OpenDialogueEvent += TalkState;

        talker.CloseDialogueEvent += PatrolState;

        if (myType == NPCTypeEnum.Enemy)
        {
            fighter.lookingToFight = true;
            fighter.FightStartedEvent += FightState;
            vision.SeePlayerBoolEvent += AttackState;
        }

        if (myType == NPCTypeEnum.Stationary)
            ChangeState(NPCStates.Idle);

        else
            ChangeState(NPCStates.Patrol);
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
        if(currentState==NPCStates.Patrol)
        if(input)
            ChangeState(NPCStates.Attack);
    }

    private void TalkState()
    {
        ChangeState(NPCStates.Talk);
    }

    private void FightState()
    {
        ChangeState(NPCStates.Fight);
    }

    private void PatrolState()
    {
        if (isAlive)
        {
            if (myType == NPCTypeEnum.Enemy)
            {
                ChangeState(NPCStates.Fight);
            }

            else if (myType == NPCTypeEnum.Stationary)
                ChangeState(NPCStates.Idle);

            else
                ChangeState(NPCStates.Patrol);
        }
        else
        {
            ChangeState(NPCStates.Death);
        }
    }

    private void Death(StatsEnum statsEnum, int i)
    {
        if (statsEnum != StatsEnum.HP)
            return;
        if (i <= 0)
        {
            ChangeState(NPCStates.Death);
        }
    }

    public void ChangeState(NPCBrain.NPCStates inputState)
    {
        if (stateDictionary.TryGetValue(inputState, out GameObject stateObject))
        {
            stateManager.ChangeState(stateObject);
        }
        else
        {
            //think state goes here
            Debug.LogError("State not found: " + inputState);
        }
    }
}