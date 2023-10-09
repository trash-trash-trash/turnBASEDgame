using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManagerSingleton : MonoBehaviour
{
    #region Singleton

    private static BattleManagerSingleton instance;

    public static BattleManagerSingleton BattleManagerSingletonInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleManagerSingleton>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("singleton");
                    instance = singletonObject.AddComponent<BattleManagerSingleton>();
                }
            }

            return instance;
        }
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion

    public List<PartyMemberScriptableObject> playerOneParty;

    public List<PartyMemberScriptableObject> playerTwoParty;

    public event Action FightStartedEvent;

    public event Action<IStartFights> NPCStartedFightEvent;

    public event Action FightEndedEvent;

    public GameObject cameraObj;

    public void NPCStartFight(IStartFights newFighter)
    {
        NPCStartedFightEvent?.Invoke(newFighter);
    }

    public void StartFight()
    {
        cameraObj.SetActive(false);

        Scene subscene = SceneManager.GetSceneByName("DealerTest");
        if (subscene.isLoaded)
        {
            return;
        }

        FightStartedEvent?.Invoke();
        SceneManager.LoadScene("DealerTest", LoadSceneMode.Additive);
        Time.timeScale = 0f;
        Scene battleScene = SceneManager.GetSceneByName("DealerTest");
        StartCoroutine(Load(battleScene));
    }

    private IEnumerator Load(Scene input)
    {
        while (!input.isLoaded)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(input);
    }

    public void SetParty(List<PartyMemberScriptableObject>targetList, List<PartyMemberScriptableObject> newList)
    {
        if(targetList==null)
            targetList=new List<PartyMemberScriptableObject>();

        targetList.Clear();
        targetList.AddRange(newList);
    }

    public void EndFight()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("OverworldTest"));
        Time.timeScale = 1f;

        Scene battleScene = SceneManager.GetSceneByName("DealerTest");
        if (battleScene.isLoaded)
        {
            SceneManager.UnloadScene(battleScene);
        }

        cameraObj.SetActive(true);

        FightEndedEvent?.Invoke();
    }
}
