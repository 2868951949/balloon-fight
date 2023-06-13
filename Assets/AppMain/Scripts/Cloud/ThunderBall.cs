using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _lastPos;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IfInScrenn())
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        _lastPos = _rigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BounceSelf(_lastPos, collision.contacts[0].normal);

        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish" || collision.gameObject.tag == "Water")
        {
            Destroy(gameObject);
        }
    }

    private bool IfInScrenn()
    {
        Vector2 sp = Camera.main.WorldToScreenPoint(transform.position);

        if (sp.y < 0 || sp.y > Screen.height)
        {
            return false;
        }

        return true;
    }

    private void BounceSelf(Vector2 inDirection, Vector2 inNormal)
    {
        Vector2 outDirection = Vector2.Reflect(inDirection, inNormal);
        //_rigidbody.velocity = outDirection.normalized;
        _rigidbody.velocity = outDirection.normalized * _lastPos.magnitude;
        //_rigidbody.AddForce(outDirection, ForceMode2D.Impulse);
    }
}
