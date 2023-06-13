using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public GameObject balloon;
    public int balloonCount = 5;

    public Transform pipes;

    private List<Transform> pivots = new List<Transform>();
    private bool isOver = false;

    private void Start()
    {
        for (int i = 0; i < pipes.childCount; i++)
        {
            pivots.Add(pipes.GetChild(i).GetChild(0).transform);
        }

        StartCoroutine(Generate());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ScoreManager.Instance.balloonCount = 0;
            TransitionManager.Instance.Transition("BonusScene", "Menu");
        }

        if (balloonCount == 0 && transform.childCount == 0 && !isOver)
        {
            isOver = true;
            ScoreManager.Instance.ShowScore();
        }
    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(2);

        while (balloonCount > 0)
        {
            int index = Random.Range(0, pivots.Count - 1);

            GameObject newBalloon = Instantiate(balloon, pivots[index]);
            newBalloon.transform.parent = transform;
            balloonCount--;

            yield return new WaitForSeconds(5f);
        }
    }


}
