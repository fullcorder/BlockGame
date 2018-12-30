﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;

    [SerializeField] private Vector2 initVelocity;

    private void Start()
    {
        rigidbody.velocity = initVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        AdjustVelocity();
    }

    private void AdjustVelocity()
    {
        var velocity = rigidbody.velocity;
        var hasChange = false;

        if(Mathf.Abs(velocity.x) < initVelocity.x)
        {
            velocity.x = velocity.x > 0f ? initVelocity.x : -initVelocity.x;
            hasChange = true;
        }

        if(Mathf.Abs(velocity.y) < 2f)
        {
            velocity.y = velocity.y > 0f ? initVelocity.y : -initVelocity.y;
            hasChange = true;
        }

        if(hasChange) rigidbody.velocity = velocity;
    }
}
