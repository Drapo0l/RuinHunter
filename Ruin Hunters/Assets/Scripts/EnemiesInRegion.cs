using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRegionEnemyPool", menuName = "Enemy Pool/Region Enemy Pool")]
public class RegionEnemyPool : ScriptableObject
{
    public PublicEnums.Regions region; // region for the enemy pool
    public GameObject collidingEnemy; // make sure to spawn enemy player collides with
    public List<GameObject> enemyPrefabs; // list of enemies
    public int maxEnemies = 3;

    //method to spawn enemy
    public List<GameObject> GetEnemies()
    {
        List<GameObject> enemiesToSpawn = new List<GameObject>();

        if (collidingEnemy != null) 
        {
            GameObject collidingInstance = GameObject.Instantiate(collidingEnemy);
            enemiesToSpawn.Add(collidingInstance);
        }

        for (int i = 0; i < maxEnemies - 1; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemy = GameObject.Instantiate(enemyPrefabs[randomIndex]);
            enemiesToSpawn.Add(enemy);
        }
        return enemiesToSpawn;
    }

}
