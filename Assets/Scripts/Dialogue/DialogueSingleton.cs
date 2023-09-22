using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSingleton : MonoBehaviour
{
    private static DialogueSingleton _instance;

    #region Singleton

     public static DialogueSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DialogueSingleton>();
                
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("singleton");
                    _instance = singletonObject.AddComponent<DialogueSingleton>();
                }
            }
            return _instance;
        }
    }
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion

    public event Action<bool> OpenCloseDialogueEvent;

    public event Action<string> NewDialogueEvent;

    public ITalk talker;
    
    public void OnNewDialogue(string input)
    {
        Debug.Log("Displaying dialogue: " + input);
        NewDialogueEvent?.Invoke(input);
    }

    public void OnOpenCloseDialogue(bool input)
    {
        OpenCloseDialogueEvent?.Invoke(input);

        if(!input && talker!=null)
            talker.CloseDialogue();
    }
}
