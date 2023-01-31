using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public BaseEnemyAI enemyPrefab;
    public Transform target;
    public float spawnDistance = 15;
    public float minInterval = 3;
    public float maxInterval = 7;
    private float spawnTime = 0;
    public float maxAngle = 10;
    public int minWaveSize = 1;
    public int maxWaveSize = 5;

    void Start()
    {
        
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if(spawnTime <= 0)
        {
            spawnTime += Random.Range(minInterval, maxInterval);
            bool waveDirection = Random.Range(0, 1.0f) > 0.5f;
            for(int i=0; i<Random.Range(minWaveSize, maxWaveSize); i++)
            {
                float angle = Random.Range(-maxAngle, maxAngle) * Mathf.PI / 180;
                Vector3 direction = new Vector3(Mathf.Cos(angle) * (waveDirection ? -1:1), 0, Mathf.Sin(angle));
                Vector3 spawnPoint = transform.position + direction * spawnDistance;
                Instantiate(enemyPrefab, spawnPoint, Quaternion.identity).target = target;
            }
        }
    }
}
