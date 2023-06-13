using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float fishSpeed = 2.0f;
    public float findRadius = 1.0f;

    private Vector2 _leftPos;
    private Vector2 _rightPos;
    private Vector2 _targetPos;

    public bool _isFindTarget;

    private void Start()
    {
        _leftPos = new Vector2(-7, transform.position.y);
        _rightPos = new Vector2(7, transform.position.y);
        _targetPos = _leftPos;

        _isFindTarget = false;
    }


    private void Update()
    {
        // 如果没吃到人，就继续找
        if (!_isFindTarget)
        {
            // 移动
            FishMove();

            // 找人
            _isFindTarget = FindTarget();
        }
        else
        {
            // 吃人
            StartCoroutine(Attack());
        }
    }

    // 移动
    private void FishMove()
    {
        if (Vector2.Distance(transform.position, _targetPos) < 0.1f)
        {
            _targetPos = _targetPos == _leftPos ? _rightPos : _leftPos;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, fishSpeed * Time.deltaTime);
        }
    }

    // 搜寻敌人
    private bool FindTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, findRadius);

        foreach (var collider in colliders)
        {
            // 如果检测区域内有敌人
            if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    // 吃人
    IEnumerator Attack()
    {
        // 调转身子
        Vector2 curpos = transform.position;
        transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(1);

        // 转回去
        transform.eulerAngles = new Vector3(0, 0, 90);
        transform.position = curpos;

        // 重置吃人状态
        _isFindTarget = false;

        yield return null;
    }

    // 绘制鱼的攻击范围
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRadius);
    }
}
