using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("战斗关卡")]
    public TMP_Text score_Text;
    public TMP_Text maxScore_Text;

    public int score = 0;
    public int maxScore = 0;

    [Header("奖励关卡")]
    public GameObject statsPanel;
    public TMP_Text balloon_Text;
    public TMP_Text tempScore_Text;

    public int balloonCount = 0;
    public int tempScore = 0;

    [Header("生命显示")]
    public GameObject power1;
    public GameObject power2;

    public int power = 3;

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
    }

    private void Update()
    {
        if (score > maxScore)
        {
            maxScore = score;
        }

        score_Text.text = string.Format("{0:D6}", score);
        maxScore_Text.text = string.Format("{0:D6}", maxScore);

        ShowPower();
    }

    // 显示生命
    public void ShowPower()
    {
        if (power == 1)
        {
            power1.SetActive(false);
            power2.SetActive(false);
        }
        else if (power == 2)
        {
            power1.SetActive(true);
            power2.SetActive(false);
        }
        else if (power == 3)
        {
            power1.SetActive(true);
            power2.SetActive(true);
        }
    }

    public void ShowScore()
    {
        StartCoroutine(AddScore());
    }

    private void OnAfterSceneLoadEvent()
    {
        statsPanel.SetActive(false);
    }

    IEnumerator AddScore()
    {
        tempScore = 700 * balloonCount;
        balloon_Text.text = balloonCount.ToString();
        tempScore_Text.text = tempScore.ToString();
        statsPanel.SetActive(true);

        yield return new WaitForSeconds(1);

        while (tempScore > 0)
        {
            yield return new WaitForSeconds(0.02f);
            tempScore -= 100;
            score += 100;

            tempScore_Text.text = tempScore.ToString();
        }

        balloonCount = 0;
        tempScore = 0;

        yield return new WaitForSeconds(3);

        TransitionManager.Instance.Transition("BonusScene", "BattleScene");
    }


}
