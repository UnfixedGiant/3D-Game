using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;

    // Patrol 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States.
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttack;

    private void Awake() 
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Patrolling()
    {

    }
    private void ChasePlayer()
    {
        
    }
    private void AttackPlayer()
    {
        
    }


    void Start()
    {
        
    }

    void Update()
    {
        // Check to see if the player is in the sight of the enemy and if they are in attack range aswell.
        playerInSight = Physics.CheckSphere(transform.position, sightRange, player);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, player);

        if (!playerInSight && !playerInAttack) Patrolling();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) AttackPlayer();

    }
}
