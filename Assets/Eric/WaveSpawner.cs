using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Wave
{
    public string waveName;
    public int numEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;

    [Tooltip("Uses the first wave values to create an infinite wave")]
    public bool infinite = false;

    private Wave currentWave;
    private int currentWaveNum;
    private float nextSpawnTime;

    private bool canSpawn = true;

    private void Update()
    {
        currentWave = waves[currentWaveNum];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        // Move on to the next wave unless it is the last wave
        if (totalEnemies.Length == 0 && !canSpawn && currentWaveNum + 1 != waves.Length)
        {
            currentWaveNum++;
            canSpawn = true;
        }
    }

    // Randomizes each enemy and location for the wave
    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            // If in a finite amount of waves, move onto the next wave
            if (!infinite)
            {
                currentWave.numEnemies--;
                if (currentWave.numEnemies == 0)
                {
                    canSpawn = false;
                }
            }
        }
    }
}
