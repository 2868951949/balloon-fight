using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现环绕房间的效果
/// </summary>
public class WrapAround : MonoBehaviour
{
    private Vector2 _screenPos;
    private Vector2 _spriteSize;

    private void Start()
    {
        // 获取物体图形的像素大小
        _spriteSize = GetComponent<SpriteRenderer>().size;
    }

    private void Update()
    {
        RoundRoom();
    }

    /// <summary>
    /// 实现环绕效果（在此步骤中，相机外的部分不会被渲染）
    /// </summary>
    public void RoundRoom()
    {
        // 物体的屏幕坐标
        _screenPos = new Vector2(
            Camera.main.WorldToScreenPoint(transform.position).x,
            Camera.main.WorldToScreenPoint(transform.position).y
            );

        // 接近左屏幕
        if (_screenPos.x < 0)
        {
            _screenPos.x = Screen.width - 1;
            transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(_screenPos).x,
                Camera.main.ScreenToWorldPoint(_screenPos).y,
                0
                );
        }

        // 接近右屏幕
        if (_screenPos.x > Screen.width)
        {
            _screenPos.x = 1;
            transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(_screenPos).x,
                Camera.main.ScreenToWorldPoint(_screenPos).y,
                0
                );
        }
    }

    /// <summary>
    /// 绘制超出相机的部分
    /// </summary>
    public void DrawSelf()
    {
        // 超出左边屏幕
        if ((_screenPos.x - _spriteSize.x * 0.5f) < 0)
        {

        }

        // 超出右边屏幕
        if ((_screenPos.x + _spriteSize.x * 0.5f) > Screen.width)
        {

        }
    }
}
