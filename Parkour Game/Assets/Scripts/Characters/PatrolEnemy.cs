using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int health;
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
        agent.destination = player.position;
    }

    private void AttackPlayer()
    {
        agent.destination = player.position;
    }


    void Start()
    {
        
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check to see if the player is in the sight of the enemy and if they are in attack range aswell.
        if (distanceToPlayer <= sightRange)
        {
            playerInSight = true;
        }
        else 
        {
            playerInSight = false;
        }
        if (distanceToPlayer <= attackRange)
        {
            playerInAttack = true;
        }
        else
        {
            playerInAttack = false;
        }

        if (!playerInSight && !playerInAttack) Patrolling();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) AttackPlayer();

    }
}
