using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever2 : MonoBehaviour
{
    public bool MoveLift3;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb.linearVelocity.x < 0f)
            MoveLift3 = true;
        else
        {
            MoveLift3 = false;
        }
    }
}
