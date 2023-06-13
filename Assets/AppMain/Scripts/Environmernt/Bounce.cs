using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 outDirection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            //rb.velocity = outDirection.normalized * rb.velocity.magnitude;
            rb.AddForce(outDirection.normalized * 5, ForceMode2D.Impulse);
        }
    }
}
