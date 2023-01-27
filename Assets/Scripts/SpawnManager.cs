using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] float waitTime;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] GameObject[] powerUps;
    

    private bool stopSpawning = false;

    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUp());
    }

    // Spawn game objects every 5 seconds
    // Create a coroutine of type IEnumerator - - Yield Events
    // while loop

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1f);
        // while loop (infinite loop)
        // Instantiate enemy prefab
        // yield wait for 5 seconds
        while (stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);            
            newEnemy.transform.parent = enemyContainer.transform;

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(1f);
        // every 3-7 sec spawn a powerup
        while (stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);

            Instantiate(powerUps[randomPowerUp], posToSpawn, Quaternion.identity);            

            yield return new WaitForSeconds(Random.Range(3f,7f));
        }

    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
}
