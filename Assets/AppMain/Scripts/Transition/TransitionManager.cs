using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    private bool isFade = false;

    public void Transition(string curScene, string nextScene)
    {
        if (!isFade)
        {
            StartCoroutine(TransitionToScene(curScene, nextScene));
        }
    }

    private IEnumerator TransitionToScene(string curScene, string nextScene)
    {
        isFade = true;

        // 离开当前场景
        if (curScene != string.Empty)
        {
            EventHandler.CallBeforeSAceneLoadEvent();
            yield return SceneManager.UnloadSceneAsync(curScene);
        }

        // 到达下一个场景
        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        // 激活新场景
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);

        EventHandler.CallAfterSceneLoadEvent();

        isFade = false;
    }
}
