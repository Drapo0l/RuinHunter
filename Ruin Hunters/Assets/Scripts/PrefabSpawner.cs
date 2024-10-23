using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public List<GameObject> spawnedPrefabs;
    public Transform spawnOrigin;
    public int maxAmount;
    public float timeBetweenSpawn;
    public GameObject enemyParent;

    private List<GameObject> spawnedObjects;

    private void Start()
    {
        spawnedObjects = new List<GameObject>();
        spawnEnemies();
    }

    private void Update()
    {
        if(spawnedObjects.Count < maxAmount)
        {
            StartCoroutine(spawnObject());
        }
    }

    private IEnumerator spawnObject()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        if (spawnedObjects.Count < maxAmount)
        {
            int randomNum = Random.Range(0, spawnedPrefabs.Count);
            GameObject gameObject = spawnedPrefabs[randomNum];

            GameObject newEnemy = Instantiate(gameObject, this.transform.position, Quaternion.identity);
            newEnemy.SetActive(true);
            newEnemy.transform.parent = enemyParent.transform;
            spawnedObjects.Add(newEnemy);
        }
    }

    private void spawnEnemies()
    {
        while(spawnedObjects.Count < maxAmount)
        {
            int randomNum = Random.Range(0, spawnedPrefabs.Count);
            GameObject gameObject = spawnedPrefabs[randomNum];

            GameObject newEnemy = Instantiate(gameObject, this.transform.position, Quaternion.identity);
            newEnemy.SetActive(true);
            newEnemy.transform.parent = enemyParent.transform;
            spawnedObjects.Add(newEnemy);
        }
    }
}
