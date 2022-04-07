using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRandomSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public AITankController enemyPrefab;

    void Start()
    {
        if(!enemyPrefab)
            enemyPrefab = Resources.Load<AITankController>("Prefabs/AITank");

        int randSpawnPoint = Random.Range(0, spawnPoints.Length);

        for(int i = 0; i < spawnPoints.Length; i++)
            Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
    }

    bool EnemiesPresent()
	{
        AITankController[] enemies = FindObjectsOfType<AITankController>();
        return enemies.Length != 0;
	}

    void Update()
    {
        
    }
}