using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBounce : MonoBehaviour
{
    private RaycastProjectile projectile;
    public float maxAngle = 30;
    public float raycastCount = 5;
    public float range = 10;
    public LayerMask layerMask;

    void Start()
    {
        for(int i=0; i<raycastCount; i++)
        {
            float angle = -maxAngle + i / (float)raycastCount * maxAngle * 2;
            Vector3 raycastDirection = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
            bool raycastFound = Physics.Raycast(transform.position, raycastDirection, range, layerMask);
            List<Vector3> validBounceDirections = new List<Vector3>();
            if(raycastFound)
            {
                // Debug.DrawLine(transform.position, transform.position + raycastDirection * range, Color.red, 5);
                validBounceDirections.Add(raycastDirection);
            }
            // else 
            //     Debug.DrawLine(transform.position, transform.position + raycastDirection * range, Color.white, 5);
            if(validBounceDirections.Count > 0)
            {
                transform.rotation = Quaternion.LookRotation(validBounceDirections[Random.Range(0, validBounceDirections.Count)]);
            }
        }
    }
}
