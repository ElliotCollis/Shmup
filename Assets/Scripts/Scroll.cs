﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour // Simple scrolling object script.
{
    [SerializeField] private float _scrollSpeed = 5f; // how fast we move
    [SerializeField] private float _startHeight = 7f; // where we start
    [SerializeField] private float _endHeight = -20f; // where we end and jump back to star

    void Start()
    {
        _startHeight = transform.position.y; // can set the start height here if it's in position already. 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = transform.position; // save a reference of the position
        targetPos.y = targetPos.y - (Time.deltaTime * _scrollSpeed); // adjust by our speed so we move.
        

        if (targetPos.y <= _endHeight) // if we're at our end position
        {
            targetPos.y = _startHeight; // jump back to the begining.
        }

        transform.position = targetPos; // otherwise keep moving.
    }
}
