using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Keybind related variables
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;


    // Movement related variables
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;

    public float wallrunSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    // Multiplier to increase player speed 
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    // Drag to make sure that the player does not slide around too much.
    public float groundDrag;

    // Jumping related Keybinds
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    // Ground check variables
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Crouching related variables
    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    // Slope related variables
    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Collectables")]
    public float coins;
    public static float totalCoins = 0;

    // Checks to see whether the player is sliding and if they are wallrunning.
    public bool sliding;
    public bool wallrunning;

    // Current movement state
    public movementState state;

    // The different movement states.
    public enum  movementState
    {
        sprinting,
        walking,
        crouching,
        sliding,
        wallrunning,
        air
    }

    public void AddCoin()
    {
        coins += 1;
        totalCoins = coins;
    }


    // Player movement states.
    private void Statehandler()
    {

        if (wallrunning)
        {
            state = movementState.wallrunning;
            moveSpeed = wallrunSpeed;
        }


        else if (sliding)
        {
            state = movementState.sliding;

            moveSpeed = slideSpeed;
        }

        else if (Input.GetKey(sprintKey) && grounded)
        {
            state = movementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        else if (Input.GetKey(crouchKey))
        {
            state = movementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded)
        {
            state = movementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = movementState.air;

        }

    }


    // Initialize the Rigidbody and the freezerotation.
    // Also stores the original Y scale for crouching and sliding.
    // So player can revert back to their original size after crouching/sliding.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

        coins = totalCoins;
    }

    // Handles player movement
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Checks to see if the player is grounded, handles ground drag and 
    // handles input, speed control and state management. 
    private void Update () 
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.4f, whatIsGround);
        MyInput();
        SpeedControl();
        Statehandler();

        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

    }

    // Smoothly transitions the player's move speed using Lerp.
    // It calculates a point between 2 positions
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            // Changes the players speed based on if they are on a slope or not.
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease; 
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }



    // This takes care of the players movement inputs whether the player is moving around with "wasd" or if they are jumping.
    // Jump also has a cooldown to make sure that the player cant jump while in mid air or spam jump.
    private void MyInput()
    {
        // Get the input for movement.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // When the player has to jump.

        if (Input.GetKey(jumpKey) && readyToJump && (grounded || OnSlope()) && !wallrunning)
        {
            readyToJump = false;
            
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Handles crouching.
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    // This handles the players actual movement.
    private void MovePlayer()
    {
        // Movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // movement if the player is on a slope.
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            // This prevents upward velocity when jumping on slopes.
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }

        // On Ground movement.
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // Handles movement while the player is in the air.
        else if(!grounded)
        {
            rb.AddForce(Vector3.down * 10f, ForceMode.Force);
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // Enable gravity only when the player is not on a slope.
        rb.useGravity = !OnSlope();
    }

    // The SpeedControl function makes sure that the player is not able to move above their movespeed.
    private void SpeedControl() 
    {
        // Limits the players speed while they are on a slope.
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        // Limits the horizontal speed.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Jumping.
    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    // Reseting Jumping.
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    // Checking to see if the player is on a slope.
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    // Gets the movemnt direction for slopes.
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

}
