using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeObject : MonoBehaviour
{
    public Transform upgradeMenuPrefab;
    public Transform menuContainer;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<KeyboardMovement>() != null)
        {
            Instantiate(upgradeMenuPrefab, menuContainer);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
