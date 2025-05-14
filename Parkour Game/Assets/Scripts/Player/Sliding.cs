using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{

    // References to player orientation, player object, rigidbody and the player movement script.
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private Player pm;

    // Variables for player sliding.

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    // Start is called before the first frame update
    // Get the player rigidbody, movement script and the original yscale of the player.(Height)
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player>();

        startYScale = playerObj.localScale.y;
    }


    // Update is called once per frame
    private void Update()
    {
        // Input values for horizontal and vertical movement.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // If there is movement input and the player is sliding they the player will slide.
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }
        // Player will stop sliding when they release the slide key.
        if(Input.GetKeyUp(slideKey) && pm.sliding)
        {
            StopSlide();
        }
    }

    // Sliding movement when the player is sliding.
    private void FixedUpdate()
    {
        if(pm.sliding)
        {
            SlidingMovement();
        }

    }

    // Method to start sliding 
    // Changes the players y scale to make it look like they are sliding, apply a downward force and
    // Set the sliding state to true.
    private void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    // Input direction based on player input
    // If the player is not on a slope or moving upwards then do normal slide force.
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        else
        // Apply slide force in the slopes direction.
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }

    }
    // Changes slide state to false and resets the players y scale(Height).
    private void StopSlide()
    {
        pm.sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }






    
}
