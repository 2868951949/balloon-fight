using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody2D rigid;

    private Vector3 lastDir;

    public float speed = 30;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector3(1, 0, 1) * speed;
    }

    private void LateUpdate()
    {
        lastDir = rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 reflexAngle = Vector3.Reflect(lastDir, collision.contacts[0].normal);
        rigid.velocity = reflexAngle.normalized * lastDir.magnitude;
    }
}
