using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    public GameObject Spawmlocation; 
    public GameObject enemy;
    private Vector3 spawnloc; 
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        Spawmlocation = GameObject.FindGameObjectWithTag("SpawnPoint");
        spawnloc = enemy.transform.position;
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        GameObject.Instantiate(enemy, Spawmlocation.transform.position, Quaternion.identity);
    }
}
