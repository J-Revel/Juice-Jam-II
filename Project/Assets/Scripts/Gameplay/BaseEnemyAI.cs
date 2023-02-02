using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    private MovementController movement;
    public Transform target;
    public float range;
    public Transform weaponTransform;
    public Transform projectilePrefab;
    private float shootTime = 0;
    public float shootInterval = 1;
    private UnityEngine.AI.NavMeshAgent agent;
    private UnityEngine.AI.NavMeshPath path;
    public NavMeshQueryFilter queryFilter;
    private int pathCursor = 0;
    public float pathfindInterval = 2;
    private float pathfindTime = 0;
    public float pathRecalculateDistance = 2;
    private Vector3 lastTargetPos;
    private Vector3 currentPathDirection;
    public LayerMask viewRaycastLayer;
    public AnimatedSprite animatedSprite;
    public StatEvaluator recoilIntensity;


    void Start()
    {
        movement = GetComponent<MovementController>();
        queryFilter.areaMask = NavMesh.AllAreas;
        queryFilter.SetAreaCost(0, 1);
        queryFilter.SetAreaCost(1, 10);
        Health health = GetComponent<Health>();
        health.hurtDelegate += (Ray hitDirectionRay) => {
            transform.position += hitDirectionRay.direction.normalized * recoilIntensity.value;
        };
    }

    void Update()
    {
        pathfindTime += Time.deltaTime;
        Vector3 targetPos = target.position;
        if(pathfindTime > pathfindInterval || Vector3.SqrMagnitude(lastTargetPos - targetPos) > pathRecalculateDistance * pathRecalculateDistance)
        {
            if(pathfindTime > pathfindInterval)
                pathfindTime -= pathfindInterval;
            lastTargetPos = targetPos;
            path = new UnityEngine.AI.NavMeshPath();
            UnityEngine.AI.NavMeshHit hit;
            Vector3 startPos = transform.position;
            Vector3 endPos = targetPos;
            if(UnityEngine.AI.NavMesh.SamplePosition(transform.position, out hit, 10, 1))
            {
                startPos = hit.position;
                Debug.DrawLine(transform.position, startPos, Color.blue, 1);
            }
            if(UnityEngine.AI.NavMesh.SamplePosition(targetPos, out hit, 10, 1))
            {
                endPos = hit.position;
                Debug.DrawLine(endPos, targetPos, Color.blue, 1);
            }
            UnityEngine.AI.NavMesh.CalculatePath(startPos, endPos, queryFilter, path);
            pathCursor = 0;
            if(path != null && path.corners.Length > 0)
                currentPathDirection = path.corners[0] - transform.position;
        }
        Vector3 direction = targetPos - transform.position;
        animatedSprite.flipX = direction.x > 0;
        if(path != null && path.corners != null && pathCursor < path.corners.Length)
        {
            Debug.DrawLine(path.corners[pathCursor], transform.position, Color.red, 0);
            for(int i=pathCursor + 1; i<path.corners.Length; i++)
                Debug.DrawLine(path.corners[i], path.corners[i-1], Color.green, 0);
            if(Vector3.SqrMagnitude(path.corners[pathCursor] - transform.position) < 1 || Vector3.Dot(currentPathDirection, path.corners[pathCursor] - transform.position) < 0)
            {
                pathCursor++;
                if(pathCursor < path.corners.Length)
                    currentPathDirection = path.corners[pathCursor] - transform.position;
                    
            }
            if(pathCursor < path.corners.Length)
                direction = currentPathDirection;
            else direction = targetPos - transform.position;
        }
        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0;
        if(targetDirection.sqrMagnitude > range * range || Physics.Raycast(transform.position, targetDirection, targetDirection.magnitude, viewRaycastLayer))
        {
            movement.inputDirection = direction.normalized;
        }
        else
        {
            movement.inputDirection = Vector3.zero;
            shootTime += Time.deltaTime;
            if(shootTime > shootInterval)
            {
                shootTime -= shootInterval;
                Instantiate(projectilePrefab, weaponTransform.position, Quaternion.LookRotation(targetDirection));
            }
        }
    }
}
