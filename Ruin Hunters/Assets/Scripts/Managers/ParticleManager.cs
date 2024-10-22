using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShootParticle(GameObject target, Transform caster, ParticleSystem par) 
    {
        if (par != null)
        {
            ParticleSystem particleInstance = GameObject.Instantiate(par, caster.position, Quaternion.identity);
            StartCoroutine(MoveParticle(particleInstance, target.transform.position));
        }
    }

    private IEnumerator MoveParticle(ParticleSystem particleInstance, Vector3 targetPosition)
    {
        float speed = 5f;

        while (particleInstance != null && Vector3.Distance(particleInstance.transform.position, targetPosition) > 0.1f) 
        {
            particleInstance.transform.position = Vector3.MoveTowards(
                particleInstance.transform.position,
                targetPosition,
                speed *  Time.deltaTime
                );

            yield return null;
        }

        if (particleInstance != null)
        {
            particleInstance.Stop();
            GameObject.Destroy(particleInstance.gameObject, 0.5f);
        }
    }
}
