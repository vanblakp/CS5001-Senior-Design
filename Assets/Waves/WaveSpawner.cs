using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable] public class Wave
{
    public string waveName;
    public int numEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public Transform[] spawnPoints;

    [Header("Infinite Wave Parameters")]
    public bool infinite = false;
    public int minEnemiesInWave = 1;
    public int maxEnemiesInWave = 10;
    public GameObject[] randomTypeOfEnemies;
    public float maxSpawnInterval = 2;

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
        if (totalEnemies.Length == 0 && !canSpawn && currentWaveNum + 1 != waves.Count)
        {
            currentWaveNum++;
            canSpawn = true;

            // If wanting to randomly make an infinite number of waves
            if (infinite)
            {
                Wave newWave = new Wave();
                newWave.numEnemies = UnityEngine.Random.Range(minEnemiesInWave, maxEnemiesInWave);
                newWave.typeOfEnemies = randomTypeOfEnemies;
                newWave.spawnInterval = UnityEngine.Random.Range(1, maxSpawnInterval);
                waves.Add(newWave);
            }
        }
    }

    // Randomizes each enemy and location for the wave
    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[UnityEngine.Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity); //randomPoint.rotation);
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            currentWave.numEnemies--;
            if (currentWave.numEnemies == 0)
            {
                canSpawn = false;
            }
        }
    }
}
