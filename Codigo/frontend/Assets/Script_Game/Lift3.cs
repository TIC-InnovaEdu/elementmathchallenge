using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift3 : MonoBehaviour
{
    [SerializeField] Lever2 s; // Referencia a Lever2
    Vector3 newposition;
    Vector3 oldPosition;

    void Start()
    {
        newposition = new Vector3(transform.position.x, transform.position.y - 4f, transform.position.z);
        oldPosition = transform.position;
    }

    void Update()
    {
        if (s.MoveLift3)
        {
            if (transform.position != newposition)
                transform.position = Vector3.Lerp(transform.position, newposition, 0.01f);
        }
        else
        {
            if (transform.position != oldPosition)
                transform.position = Vector3.Lerp(transform.position, oldPosition, 0.01f);
        }
    }
}

