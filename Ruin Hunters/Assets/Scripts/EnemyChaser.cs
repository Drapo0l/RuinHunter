using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
       
        flipTransform = enemyTransform;
        flipTransform.transform.localScale = new Vector3(-enemyTransform.localScale.x, enemyTransform.localScale.y, enemyTransform.localScale.z);
    }


    void Update()
    {
        // Prevent rotation by overriding it to always face a certain direction       
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        isinSight = Physics.CheckSphere(transform.position, Sightrange, WherePlayer);
        if (!isinSight)
        {
            enemyAnimator.SetBool("isChasing", false);
            Patroling();

        }
        if (isinSight)
        {
            enemyAnimator.SetBool("isChasing", true);
            Chase();
        }
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
