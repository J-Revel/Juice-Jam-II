using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public EntityPhysicsConfig physicsConfig;
    public Vector3 inputDirection;
    private new Rigidbody rigidbody;
    public float dashDistance = 2;
    public float speedMultiplier = 1;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = physicsConfig.mass;
    }
    
    void FixedUpdate()
    {
        rigidbody.AddForce(inputDirection * physicsConfig.acceleration, ForceMode.Acceleration);
        rigidbody.velocity = rigidbody.velocity * Mathf.Pow(physicsConfig.inertia, Time.fixedDeltaTime);
        if(rigidbody.velocity.sqrMagnitude > physicsConfig.maxSpeed * physicsConfig.maxSpeed * speedMultiplier * speedMultiplier)
        {
            rigidbody.velocity = rigidbody.velocity * physicsConfig.maxSpeed * speedMultiplier / rigidbody.velocity.magnitude;
        }
    }

    public void Dash(Vector3 direction)
    {
        rigidbody.position += dashDistance * direction.normalized;
    }
}
