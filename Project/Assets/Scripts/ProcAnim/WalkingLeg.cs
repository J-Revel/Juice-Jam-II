using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingLeg : MonoBehaviour
{
    private ProceduralWalkConfig config;
    private Vector3 currentFootPos;
    public Vector3 footPos { get { return currentFootPos; } }
    public float stepDistanceFactor = 1;
    
    IEnumerator Start()
    {
        config = GetComponentInParent<ProceduralWalkConfig>();
        currentFootPos = transform.position;
        while(true)
        {
            float stepDistance = stepDistanceFactor * config.stepDistance;
            currentFootPos = LegTargetHeight.instance.RaycastPoint(currentFootPos);
            Vector3 direction = transform.position - currentFootPos;
            direction.y = 0;
            if(Vector3.SqrMagnitude(direction) > stepDistance * stepDistance)
            {
                Vector3 nextStepPos = currentFootPos + (transform.position - currentFootPos).normalized * (stepDistance * (1+config.overshootFactor) + Random.Range(-config.stepDistanceVariation, config.stepDistanceVariation));
                Vector3 overshootDirection = transform.position - currentFootPos;
                Vector3 startFootPos = currentFootPos;
                for(float time=0; time < config.stepDuration; time += Time.deltaTime)
                {
                    float f = time / config.stepDuration;
                    nextStepPos = transform.position + overshootDirection * config.overshootFactor;
                    currentFootPos = LegTargetHeight.instance.RaycastPoint(Vector3.Lerp(startFootPos, nextStepPos, f)) + Vector3.up * Mathf.Sin(f * Mathf.PI) * config.stepHeight;
                    yield return null;
                    // if(Vector3.SqrMagnitude(transform.position - currentFootPos) > stepDistance * stepDistance * 2)
                    // {
                    //     startFootPos = currentFootPos;
                        
                    //     time = config.stepDuration / 2;
                    // }
                }
                currentFootPos = LegTargetHeight.instance.RaycastPoint(nextStepPos);
            }
            yield return null;
        }
        
    }
}
