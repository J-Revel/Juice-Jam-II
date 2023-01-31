using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : MonoBehaviour
{
    public float speed = 2;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }
}
