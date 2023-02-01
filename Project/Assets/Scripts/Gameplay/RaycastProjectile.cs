using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProjectile : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public LayerMask raycastMask;
    private MaterialPropertyBlock propertyBlock;
    public float range = 10;
    public float animDuration = 0.3f;
    public Transform impactPrefab;
    public float damage = 1;

    IEnumerator Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.GetPropertyBlock(propertyBlock);
        lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range, raycastMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            animDuration = animDuration * hit.distance / range;
            Health health = hit.collider.GetComponentInParent<Health>() ;
            if(health != null)
            {
                health.OnDamageTaken(damage, new Ray(hit.point, transform.forward));
            }
            Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.collider.transform);
        }
        else 
            lineRenderer.SetPosition(1, transform.position + transform.forward * range);
        for(float time = 0; time < animDuration; time += Time.deltaTime)
        {
            propertyBlock.SetFloat("_AnimRatio", time/animDuration);
            lineRenderer.SetPropertyBlock(propertyBlock);
            yield return null;
        }
        Destroy(gameObject);
    }

    void Update()
    {
    }
}
