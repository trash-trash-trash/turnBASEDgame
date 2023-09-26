using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    public GameObjectStateManager stateManager;

    public GameObject AttackObj;

    public GameObject FollowObj;

    public GameObject PatrolObj;

    public GameObject TalkObj;

    public GameObject DeathObj;

    public Dictionary<NPCStates, GameObject> stateDictionary = new Dictionary<NPCStates, GameObject>();

    public Talker talker;

    public StatsBase stats;

    public enum NPCStates
    {
        Attack,
        Follow,
        Patrol,
        Talk,
        Death
    }

    public NPCStates state;

    private void OnEnable()
    {
        stats.DeclareStatEvent += Death;

        stateDictionary.Add(NPCStates.Attack, AttackObj);
        stateDictionary.Add(NPCStates.Follow, FollowObj);
        stateDictionary.Add(NPCStates.Patrol, PatrolObj);
        stateDictionary.Add(NPCStates.Talk, TalkObj);
        stateDictionary.Add(NPCStates.Death, DeathObj);

        talker.OpenDialogueEvent += TalkState;

        talker.CloseDialogueEvent += PatrolState;

        ChangeState(NPCStates.Patrol);
    }

    private void OnDisable()
    {
        stats.DeclareStatEvent -= Death;

        talker.OpenDialogueEvent -= TalkState;

        talker.CloseDialogueEvent -= PatrolState;
    }

    public void ChangeTestState()
    {
        ChangeState(state);
    }

    private void TalkState()
    {
        ChangeState(NPCStates.Talk);
    }

    private void PatrolState()
    {
        ChangeState(NPCStates.Patrol);
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