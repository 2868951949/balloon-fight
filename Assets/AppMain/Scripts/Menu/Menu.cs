using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // 进入战斗关卡
    public void EnterBattleScene()
    {
        ScoreManager.Instance.score = 0;
        TransitionManager.Instance.Transition("Menu", "BattleScene");
    }

    // 进入奖励关卡
    public void EnterBounsScene()
    {
        ScoreManager.Instance.score = 0;
        TransitionManager.Instance.Transition("Menu", "BonusScene");
    }


    // 退出游戏
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
