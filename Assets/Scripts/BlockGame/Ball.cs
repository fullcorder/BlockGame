using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ballRigidBody2D;

    [SerializeField] public Vector2 initVelocity;

    public Vector2 Velocity
    {
        get => ballRigidBody2D.velocity;
        set => ballRigidBody2D.velocity = value;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        AdjustVelocity();
    }

    private void AdjustVelocity()
    {
        var velocity = ballRigidBody2D.velocity;

        var hasChange = false;

        if(Mathf.Abs(velocity.x) < 1f || 3f < Mathf.Abs(velocity.x))
        {
            velocity.x = velocity.x > 0f ? initVelocity.x : -initVelocity.x;
            hasChange = true;
        }

        if(Mathf.Abs(velocity.y) < 1f || 3f < Mathf.Abs(velocity.y))
        {
            velocity.y = velocity.y > 0f ? initVelocity.y : -initVelocity.y;
            hasChange = true;
        }

        if(hasChange) ballRigidBody2D.velocity = velocity;
    }
}
