using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;

    public float wallRunForce;
    public float maxWallRunTime;

    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header ("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    private Player pm;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player>();
        
    }


    // Update is called once per frame

    // Check for walls around the player and handles state machine.
    void Update()
    {
        CheckForWall();
        StateMachine();
    } 
    // Wall running movement.
    private void FixedUpdate()
    {
        if(pm.wallrunning)
        {
            WallRunningMovement();
        }
    }


    // Checks for walls by casting a ray to the right and to the left.
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);

    }
    // Checks to see if the player is above ground by
    // Casting a ray downwards.
    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    // Start wall running if the player is not already wall running.
    private void StateMachine()
    {
        // Getting the inputs.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // Near a wall, moving forward and above the ground.

        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            // State = wallrunning
            if (!pm.wallrunning)
            {
                StartWallRun();
            }
        }
        else
        {
            // Stop wall running.
            if(pm.wallrunning)
                StopWallRun();
        }

    }
    
    // Wall running state.
    private void StartWallRun()
    {
        pm.wallrunning = true;

    }
    // Handles wall running while moving.
    // Maintains the players horizontal and forward vector while stopping vertical velocity.
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // Check to see if the wall forward direction aligns with the players orientation.
        if (Vector3.Dot(orientation.forward, wallForward) < 0)
        {
            wallForward = -wallForward;
        }
        // Apply force to move the player along the wall.
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    // Change the wall running state to false.
    private void StopWallRun()
    {
        pm.wallrunning = false;
    }
    
}
