using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour // simple movement script.
{
    public float speed; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed; // set the velocity of the object to be our speed.  only moves in up or down 2D directions.
    }
}
