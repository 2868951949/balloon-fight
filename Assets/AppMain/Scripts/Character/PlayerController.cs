using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
    IDEL,
    FLY,
    FALL,
    DEATH
}

public class PlayerController : Singleton<PlayerController>
{
    public Vector2 startPos;    // 玩家的出生位置

    public PlayerState playerState;
    public int power = 3;
    public int balloon = 2;     // 玩家的颜色表示剩余的气球的数量

    public float flyThrust = 12f;
    public float walkThrust = 5f;


    private Rigidbody2D _rigidbody;
    private Vector2 _lastPos;       // 上一帧角色的位置

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position = startPos;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
    }

    private void Start()
    {
        playerState = PlayerState.IDEL;
    }

    private void Update()
    {
        // 保持玩家旋转状态
        transform.rotation = Quaternion.identity;

        if (playerState != PlayerState.FALL && playerState != PlayerState.DEATH)
        {
            if (CheckIfOnGround())
            {
                playerState = PlayerState.IDEL;
            }
            else
            {
                playerState = PlayerState.FLY;
            }
        }

        // 判断玩家状态
        CheckPlayerState();

        // 根据气球数量显示对应的颜色
        UpdatePlayerSprite();

        ScoreManager.Instance.power = power;
    }

    // 根据玩家状态进行行动 
    private void CheckPlayerState()
    {
        switch (playerState)
        {
            case PlayerState.IDEL:
                {
                    // 玩家可以移动
                    if (Input.GetKey(KeyCode.J))
                    {
                        _rigidbody.AddForce(Vector2.up * flyThrust);
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        _rigidbody.AddForce(Vector2.left * walkThrust);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        _rigidbody.AddForce(Vector2.right * walkThrust);
                    }
                    break;
                }
            case PlayerState.FLY:
                {
                    // 玩家可以移动
                    if (Input.GetKey(KeyCode.J))
                    {
                        if (Input.GetKey(KeyCode.A))
                        {
                            _rigidbody.AddForce((Vector2.up + Vector2.left) * flyThrust);
                        }
                        else if (Input.GetKey(KeyCode.D))
                        {
                            _rigidbody.AddForce((Vector2.up + Vector2.right) * flyThrust);
                        }
                        else
                        {
                            _rigidbody.AddForce(Vector2.up * flyThrust);
                        }
                    }
                    break;
                }
            case PlayerState.FALL:
                {
                    // 玩家下落
                    GetComponent<Collider2D>().enabled = false;
                    _rigidbody.velocity = Vector2.down * 5;
                    GetComponent<SpriteRenderer>().sortingOrder = 6;

                    // 如果掉出屏幕外
                    if (Camera.main.WorldToScreenPoint(transform.position).y < 0)
                    {
                        playerState = PlayerState.DEATH;
                    }
                    break;
                }
            case PlayerState.DEATH:
                {
                    if (power > 1)
                    {
                        power--;
                        Reborn();
                    }
                    else
                    {
                        //Debug.Log("游戏结束");
                        GameManager.Instance.GameOver();
                    }
                    break;
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果不在地面上，则发生碰撞
        if (!CheckIfOnGround())
        {
            BounceSelf(_lastPos, collision.contacts[0].normal);
        }

        // 如果碰到了敌人
        if (collision.gameObject.tag == "Enemy")
        {
            // 如果玩家在上方
            if (IsOnTheEnemy(gameObject, collision.gameObject))
            {
                // 加分
                ScoreManager.Instance.score += 500;


                // 更改敌人状态
                EnemyState state = collision.gameObject.GetComponent<Enemy>().enemyState;

                if (state == EnemyState.FLY)
                {
                    collision.gameObject.GetComponent<Enemy>().enemyState = EnemyState.FALL;
                }
                else if (state == EnemyState.FALL || state == EnemyState.IDLE)
                {
                    collision.gameObject.GetComponent<Enemy>().enemyState = EnemyState.DEATH;
                }
            }
            else
            {
                // 玩家气球-1
                balloon--;
                if (balloon == 0)
                {
                    GetComponent<Collider2D>().enabled = false;
                    playerState = PlayerState.FALL;
                }
            }
        }

        if (collision.gameObject.tag == "Thunder")
        {
            balloon = 0;
            GetComponent<Collider2D>().enabled = false;
            playerState = PlayerState.FALL;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Water" || collision.gameObject.tag == "Fish")
        {
            GetComponent<Collider2D>().enabled = false;
            playerState = PlayerState.FALL;
        }

        if (collision.gameObject.tag == "Bubble")
        {
            ScoreManager.Instance.score += 500;
        }

        if (collision.gameObject.tag == "Balloon")
        {
            ScoreManager.Instance.balloonCount++;
        }
    }

    private void OnAfterSceneLoadEvent()
    {
        power = 3;
        balloon = 2;

        transform.position = startPos;
        _rigidbody.velocity = Vector2.zero;
    }

    // 玩家重生
    public void Reborn()
    {
        transform.position = startPos;
        GetComponent<Collider2D>().enabled = true;
        playerState = PlayerState.IDEL;
        _rigidbody.velocity = Vector2.zero;
        balloon = 2;
    }

    // 判断玩家是否在敌人上方
    private bool IsOnTheEnemy(GameObject player, GameObject enemy)
    {
        Vector2 offset = enemy.transform.position - transform.position;
        return Vector2.Dot(Vector2.up, offset) > 0 ? false : true;
    }

    // 根据气球数量显示对应的颜色
    private void UpdatePlayerSprite()
    {
        if (balloon == 2)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (balloon == 1)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }

    }

    // 判断玩家是否在地面上
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
        //_rigidbody.AddForce(outDirection.normalized, ForceMode2D.Impulse);
    }

    // 获取角色上一帧的位置
    private void LateUpdate()
    {
        _lastPos = _rigidbody.velocity;
    }
}
