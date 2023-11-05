using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNewGame : MainMenuBase
{
    public SceneAsset targetScene;

    // Add a method to load the target scene
    public void LoadTargetScene()
    {
        if (targetScene != null)
        {
            string sceneName = targetScene.name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Target scene is not assigned.");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        LoadTargetScene();
    }
}
