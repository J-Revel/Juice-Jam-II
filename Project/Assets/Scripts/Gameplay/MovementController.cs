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
    public DashVFXDisplay dashVFXPrefab;
    public float dashFXCount = 5;

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
        for(int i=0; i<dashFXCount; i++)
        {
            DashVFXDisplay vfxDisplay = Instantiate(dashVFXPrefab, Vector3.Lerp(rigidbody.position, rigidbody.position + dashDistance * direction.normalized, i / (float)dashFXCount), Quaternion.identity);
            vfxDisplay.indexRatio = i / (dashFXCount-1);
        }
        rigidbody.position += dashDistance * direction.normalized;
    }
}
