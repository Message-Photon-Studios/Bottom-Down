using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MiniBossSpawner : MonoBehaviour
{   

    [SerializeField] GameObject wisp;
    [SerializeField] GameObject[] spawnEnemies;
    [SerializeField] int maxSpawnedEnemies;
    [SerializeField] float spawnTime;
    [SerializeField] GameObject bossEnemy;

    float spawnTimer = 0;

    List<EnemyStats> spawnedEnemies = new List<EnemyStats>();    

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if(bossEnemy == null)
        {
            if(spawnedEnemies.Count > 0)
            {
                while(spawnedEnemies.Count > 0)
                {
                    if(spawnedEnemies[0] != null)
                        spawnedEnemies[0].DamageEnemy(1000000);
                }
            }

            return;
        }
        
        if(spawnedEnemies.Count < maxSpawnedEnemies)
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0)
            {
                spawnedEnemies.Add(null);
                SpawnWisp();
                spawnTimer = spawnTime;
            }
        }
    }


    private void SpawnWisp()
    {
        Debug.Log("Spawn wisp");
        GameObject spwnWisp = GameObject.Instantiate(wisp, transform.position, transform.rotation);
        spwnWisp.GetComponent<Wisp>().onObjectSpawned.AddListener(EnemySpawned);
        spwnWisp.GetComponent<Wisp>().SetSpawnObject(spawnEnemies[Random.Range(0, spawnEnemies.Count())]);
        spwnWisp.GetComponent<EnemyStats>().SetColor(LevelManager.instance.GetEnemyManager().GetRandomEnemyColor());
    }

    private void EnemySpawned(GameObject enemy)
    {
        spawnedEnemies[spawnedEnemies.Count-1] = enemy.GetComponent<EnemyStats>();
        enemy.GetComponent<EnemyStats>().onEnemyDeath += EnemyDied;
    }

    private void EnemyDied(EnemyStats deadEnemy)
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if(spawnedEnemies[i] == null || spawnedEnemies[i].IsDead()) 
            {
                if(spawnedEnemies[i] != null)
                    spawnedEnemies[i].onEnemyDeath -= EnemyDied;
                spawnedEnemies.RemoveAt(i);
                i--;
            }
        }
    }
}
