using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public enum EnemyState
{
    IDLE,
    FLY,
    FALL,
    DEATH
}

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public EnemyState enemyState;   // 敌人的状态
    public float emenySpeed = 2f;   // 敌人的移动速度

    public GameObject bubble;       // 敌人落入水中产生的泡泡

    private Rigidbody2D _rigidbody;
    private Vector2 _targetPos;     // 目标飞行位置
    private Vector2 _lastPos;       // 上一帧角色的位置
    private bool isStartPrepareFly; // 用来控制协程的启动


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 游戏一开始敌人都是 IDEL 状态
        enemyState = EnemyState.IDLE;
    }

    private void Update()
    {
        // 保持玩家旋转状态
        transform.rotation = Quaternion.identity;

        // 根据敌人状态进行动作
        CheckEnemyState();
    }

    // 根据敌人状态进行动作
    private void CheckEnemyState()
    {
        switch (enemyState)
        {
            case EnemyState.IDLE:
                {
                    // 更改显示
                    GetComponent<SpriteRenderer>().color = Color.white;

                    if (!isStartPrepareFly)
                    {
                        // 开始吹气球
                        StartCoroutine(PrepareFly());
                        isStartPrepareFly = true;
                    }
                    break;
                }
            case EnemyState.FLY:
                {
                    // 更改显示
                    GetComponent<SpriteRenderer>().color = Color.green;

                    // 向目标位置移动
                    if (Vector2.Distance(transform.position, _targetPos) < 0.1f)
                    {
                        _targetPos = GenerateNewTargetPos();
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, _targetPos, emenySpeed * Time.deltaTime);
                    }
                    break;
                }
            case EnemyState.FALL:
                {
                    // 更改显示
                    GetComponent<SpriteRenderer>().color = Color.yellow;

                    _rigidbody.velocity = Vector2.down * emenySpeed;

                    // 如果落到地面上
                    if (CheckIfOnGround())
                    {
                        // 转变为默认状态
                        enemyState = EnemyState.IDLE;
                        isStartPrepareFly = false;
                    }
                    break;
                }
            case EnemyState.DEATH:
                {
                    // 更改显示
                    GetComponent<SpriteRenderer>().color = Color.red;
                    GetComponent<SpriteRenderer>().sortingOrder = 6;

                    StopAllCoroutines();
                    enemyState = EnemyState.DEATH;

                    // 敌人下落
                    GetComponent<Collider2D>().enabled = false;
                    _rigidbody.velocity = Vector2.down * emenySpeed;


                    // 如果掉出屏幕外
                    if (Camera.main.WorldToScreenPoint(transform.position).y < 0)
                    {
                        Instantiate(bubble, transform.position, Quaternion.identity);
                        Destroy(gameObject);
                    }

                    break;
                }
        }
    }

    // 获取角色上一帧的位置
    private void LateUpdate()
    {
        _lastPos = _rigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果是飞行状态
        if (enemyState == EnemyState.FLY)
        {
            // 更改目标位置
            _targetPos = GenerateNewTargetPos();

            // 反弹效果
            BounceSelf(_lastPos, collision.contacts[0].normal);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (enemyState == EnemyState.FLY)
        {
            // 更改目标位置
            _targetPos = GenerateNewTargetPos();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果碰到鱼，直接死，而且没有泡泡
        if (collision.gameObject.tag == "Fish")
        {
            // 敌人死亡
            enemyState = EnemyState.DEATH;
            Destroy(gameObject);
        }

        // 如果碰到了水，产生泡泡
        if (collision.gameObject.tag == "Water")
        {
            // 在敌人落水的位置生成一个泡泡
            Instantiate(bubble, transform.position, Quaternion.identity);

            // 敌人死亡
            enemyState = EnemyState.DEATH;
            Destroy(gameObject);
        }
    }

    // IDEL状态，准备起飞
    IEnumerator PrepareFly()
    {
        // 等待3秒
        yield return new WaitForSeconds(3);

        // 定好一个目标位置
        _targetPos = GenerateNewTargetPos();

        // 起飞
        enemyState = EnemyState.FLY;
    }

    // 判断敌人是否落在地面上
    private bool CheckIfOnGround()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = Vector2.down;
        float maxDistance = GetComponent<Collider2D>().bounds.extents.y + 0.1f;
        LayerMask layerMask = LayerMask.GetMask("Platform");

        return Physics2D.Raycast(rayOrigin, rayDirection, maxDistance, layerMask);
    }

    // 反弹效果
    private void BounceSelf(Vector2 inDirection, Vector2 inNormal)
    {
        Vector2 outDirection = Vector2.Reflect(inDirection, inNormal);
        //_rigidbody.velocity = outDirection.normalized;
        _rigidbody.velocity = outDirection.normalized * _lastPos.magnitude;
        //_rigidbody.AddForce(outDirection, ForceMode2D.Impulse);
    }

    // 生成新的目标位置
    private Vector2 GenerateNewTargetPos()
    {
        return new Vector2(Random.Range(-8, 8), Random.Range(-2, 4));
    }

    // 在 Scene 标出目标位置
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(_targetPos, 0.1f);
    //}
}
