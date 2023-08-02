using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesManager : DucSingleton<ScenesManager>
{
    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    public void GetScene(string asset_name, bool is_additive)
    {
        SceneManager.LoadScene(asset_name, is_additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    public AsyncOperation GetSceneAsync(string asset_name, bool is_additive, Action<AsyncOperation> action, bool is_allow_activation = false)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(asset_name, is_additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = is_allow_activation;
        asyncOperation.completed += action;
        return asyncOperation;
    }

    public void UnLoadAllScene()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        int scene_count = SceneManager.sceneCount;
        for (int i = 0; i < scene_count; i++)
        {
            string scene_name = SceneManager.GetSceneAt(i).name;
            if (!scene_name.Equals(activeScene))
            {
                SceneManager.UnloadSceneAsync(scene_name);
            }
        }
        SceneManager.UnloadSceneAsync(activeScene);
    }

    public void UnLoadAllOtherScene(string currentScene, string scene2 = "", UnityAction callback = null)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        int scene_count = SceneManager.sceneCount;
        for (int i = 0; i < scene_count; i++)
        {
            string scene_name = SceneManager.GetSceneAt(i).name;
            if (!scene_name.Equals(currentScene) && !scene_name.Equals(scene2))
            {
                SceneManager.UnloadSceneAsync(scene_name);
            }
        }
        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public AsyncOperation UnLoadScene(string currentScene, UnityAction action = null)
    {
        AsyncOperation async = null;
        int scene_count = SceneManager.sceneCount;
        for (int i = 0; i < scene_count; i++)
        {
            string scene_name = SceneManager.GetSceneAt(i).name;
            if (scene_name.Equals(currentScene))
            {
                async = SceneManager.UnloadSceneAsync(scene_name);
                if (action != null)
                    action.Invoke();
                break;
            }
        }
        return async;
    }

    public AsyncOperation UnLoadDuplicateScene(string currentScene, UnityAction action = null)
    {
        int count_same_scene = 0;
        AsyncOperation async = null;
        int scene_count = SceneManager.sceneCount;
        for (int i = 0; i < scene_count; i++)
        {
            string scene_name = SceneManager.GetSceneAt(i).name;
            if (scene_name.Equals(currentScene))
            {
                count_same_scene++;
                if (count_same_scene >= 2)
                {
                    async = SceneManager.UnloadSceneAsync(scene_name);
                    if (action != null)
                        action.Invoke();
                    break;
                }
            }
        }
        return async;
    }
}
