using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DependencyManager : MonoBehaviour
{
    private static DependencyManager instance;

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
