using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public List<GameObject> spawnedPrefabs;
    public Transform spawnOrigin;
    public int maxAmount;
    public float spawnRadius;
    public float timeBetweenSpawn;
    public GameObject enemyParent;

    private List<GameObject> spawnedObjects;

    private void Start()
    {
        spawnedObjects = new List<GameObject>();
        StartCoroutine(spawnEnemies());
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

            Vector3 spawnPosition = GetRandomPositionRadius(spawnOrigin.position, spawnRadius);

            GameObject newEnemy = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            newEnemy.SetActive(true);
            newEnemy.transform.parent = enemyParent.transform;
            spawnedObjects.Add(newEnemy);
        }
    }

    private IEnumerator spawnEnemies()
    {
        while(spawnedObjects.Count < maxAmount)
        {
            yield return StartCoroutine(spawnObject());
        }
    }

    private Vector3 GetRandomPositionRadius(Vector3 origin, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float distanceFromCenter = Random.Range(0f, radius);

        float x = origin.x + Mathf.Cos(angle) * distanceFromCenter;
        float z = origin.z + Mathf.Sin(angle) * distanceFromCenter;

        return new Vector3(x, origin.y , z);
    }
}
