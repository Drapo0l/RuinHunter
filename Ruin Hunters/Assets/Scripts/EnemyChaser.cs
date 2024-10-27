using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyChaser : MonoBehaviour
{

    
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent agent;
    public LayerMask Ground, WherePlayer;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Rigidbody rb;
    [SerializeField] UnityEngine.Transform enemyTransform;
    private UnityEngine.Transform flipTransform;
    [SerializeField] RegionEnemyPool colliderPool;

    //Patroling

    public Vector3 WalkPoint;
    bool IsWalking;
    bool isCheckingFlip = false;
    [SerializeField] float walkpointRange;
  
    //States
    [SerializeField] float Sightrange;
    bool isinSight;

    [SerializeField] AudioSource Aud;
    [SerializeField] AudioClip walkSound;
    [SerializeField] float AudioWalkVol;
    private bool isWalkingSoundPlaying = false;

    void Start()
    {
       
        flipTransform = enemyTransform;
        flipTransform.transform.localScale = new Vector3(-enemyTransform.localScale.x, enemyTransform.localScale.y, enemyTransform.localScale.z);
        Aud.clip = walkSound;
        Aud.volume = AudioWalkVol;
    }


    void Update()
    {
        // Prevent rotation by overriding it to always face a certain direction       
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (!isWalkingSoundPlaying)
        {
            StartCoroutine(PlayWalkingSound());
        }

        isinSight = Physics.CheckSphere(transform.position, Sightrange, WherePlayer);
        if (!isinSight)
        {
            if (HasParameter(enemyAnimator, "Run"))
            {
                enemyAnimator.SetBool("Run", false);
                Patroling();
            }

        }
        if (isinSight)
        {
            if (HasParameter(enemyAnimator, "Run"))
            {
                enemyAnimator.SetBool("Run", true);
                Chase();
            }
        }
    }

    bool HasParameter(Animator animator, string paramName)
    {
        if (animator.parameters.Length != 0)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        playerController isPlayer = collision.GetComponent<playerController>();
       if (isPlayer != null)
        {
            Destroy(gameObject);
            GameManager.Instance.SetEnemyPool(colliderPool);
            GameManager.Instance.lastPlayerPosition = collision.transform.position;
            GameManager.Instance.combat = true;            
        }
    }

    public void Patroling()
    {
        if (!IsWalking)
        {
            SearchWalkpath();
        }
        if (IsWalking)
        { 
            agent.SetDestination(WalkPoint);
        }

        Vector3 DistanceWalking = transform.position - WalkPoint;

        if (DistanceWalking.magnitude < 1f)
        {
            IsWalking = false;
        }
    }
    private void SearchWalkpath()
    {
        float RandomZ = Random.Range(-walkpointRange, walkpointRange);
        float RandomX = Random.Range(-walkpointRange, walkpointRange);
        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (!isCheckingFlip)
        {
            StartCoroutine(CheckForFlip(WalkPoint));
           
        }

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, Ground))
        {
            IsWalking = true;
        }

    }
    
    private IEnumerator PlayWalkingSound()
    {
        isWalkingSoundPlaying = true;
        Aud.Play();
        yield return new WaitForSeconds(Aud.clip.length);
        isWalkingSoundPlaying = false;
    }

    public void Chase()
    {
        if(!isCheckingFlip)
        {
            StartCoroutine(CheckForFlip(PartyManager.Instance.CurrentActiveCharacter().transform.position));
        }
        agent.SetDestination(PartyManager.Instance.CurrentActiveCharacter().transform.position);
    }

    private IEnumerator CheckForFlip(Vector3 target)
    {
        isCheckingFlip = true;
        //get target pos        
        Vector3 directionToPlayer = target - transform.position;        
        
        //flip sprite
        if (directionToPlayer.x < 0.5f)
        {
            //player is to the left
            Model.transform.localScale = new Vector3(-Mathf.Abs(enemyTransform.localScale.x), enemyTransform.localScale.y, enemyTransform.localScale.z); // face left
        }
        if (directionToPlayer.x > -0.5f)
        {
            //player is to the left
            Model.transform.localScale = new Vector3(Mathf.Abs(enemyTransform.localScale.x), enemyTransform.localScale.y, enemyTransform.localScale.z); // face left
        }

        //delay to prevent flashing
        yield return new WaitForSeconds(0.2f);

        isCheckingFlip = false;

    }
}
