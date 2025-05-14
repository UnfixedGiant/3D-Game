using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;

    public GameObject Player {get => player; }
    public NavMeshAgent Agent {get => agent; }
    public Path path;
    [Header("Sight")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;
    [Header("Weapon Values")]
    public Transform gunBarrel1;
    public Transform gunBarrel2;
    [Range(0.1f,10f)]
    public float fireRate;

    [SerializeField]
    private string currentState;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine.Initialise();

        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
        
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            //Is the player close enough to the enemy to be seen
            if(Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                // Calculating the angle to the player
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                // Checking to see if the angle is in fov of enemy.
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    // Enemy line of sight is blocked by object?
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if(hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
