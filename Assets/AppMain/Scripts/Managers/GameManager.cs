using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject gameoverPanel;

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += Restart;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= Restart;
    }

    private void Start()
    {
        // 把开始菜单加载进来
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }

    public void GameOver()
    {
        gameoverPanel.SetActive(true);
    }

    public void Restart()
    {
        gameoverPanel?.SetActive(false);
    }
}
