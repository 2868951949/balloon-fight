using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public GameObject enemys;
    private bool isOver = false;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TransitionManager.Instance.Transition("BattleScene", "Menu");
        }

        if (enemys.transform.childCount == 0 && !isOver)
        {
            TransitionManager.Instance.Transition("BattleScene", "BonusScene");
            isOver = true;
        }
    }
}
