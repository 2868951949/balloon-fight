using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private Vector2 targetPos;

    private void Start()
    {
        targetPos = new Vector2(transform.position.x, 6f);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, 2 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AirWall")
        {
            // 销毁气球
            Destroy(gameObject);
        }
    }
}
