using UnityEngine;

public class DistanceJoint3D : MonoBehaviour {

    public Transform ConnectedRigidbody;
    public float maxRotationSpeed = 90;
    private float distance;

    void Awake()
    {
        
    }

    void Start()
    {
        distance = Vector3.Magnitude(transform.position - ConnectedRigidbody.position);
    }

    void FixedUpdate()
    {
        var anchorDirection = ConnectedRigidbody.position - transform.position;

        transform.position = ConnectedRigidbody.position - anchorDirection.normalized * distance;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, anchorDirection);
        float angle = Vector3.Angle(transform.forward, anchorDirection);
        transform.rotation = Quaternion.LookRotation(anchorDirection, Vector3.up);
    }
}
