using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Variables

    // Takes the players current orientation.
    public Transform orientation;
    // Player will be able to set a sensitivity for bot their x and y.
    // MAYBE JUST CHANGE TO 1 SENS FOR THE PLAYER.(COMPLICATED FOR NEW PLAYERS)
    public float sensX;
    public float sensY;
    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float inputY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += inputX;
        xRotation -= inputY;
        // Locks the camera so that it cant rotate over 90 degrees.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
