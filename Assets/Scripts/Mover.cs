using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour // simple movement script.
{
    [SerializeField] private float _speed; 

    private Rigidbody2D rigidD;

    private void Awake()
    {
        rigidD = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rigidD.velocity = transform.up * _speed; // set the velocity of the object to be our speed.  only moves in up or down 2D directions.
    }
}
