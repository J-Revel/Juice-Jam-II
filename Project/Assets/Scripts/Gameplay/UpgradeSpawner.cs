using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    public float spawnTime = 10;
    public float spawnInterval = 30;
    public GameObject[] upgradeElements;
    private List<GameObject> availableUpgrades;

    void Start()
    {
        availableUpgrades = new List<GameObject>();
        for(int i=0; i<transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);
            availableUpgrades.Add(child);
        }
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if(spawnTime < 0)
        {
            spawnTime += spawnInterval;
            int index = Random.Range(0, availableUpgrades.Count);
            availableUpgrades[index].SetActive(true);
            availableUpgrades.RemoveAt(index);
        }
    }
}
