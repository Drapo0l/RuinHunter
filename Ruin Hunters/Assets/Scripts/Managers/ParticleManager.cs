using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
  
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MakeParticel(GameObject position, ParticleSystem par) { 
       GameObject.Instantiate(par, position.transform.position, Quaternion.identity);
      Object.Destroy(position, 1f);
    }
}
