using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is just interacting with an object.
// Called afterlife since it was going to be seperate dimension.
// It removes objects in the scene which allows the player to move to other locations.
// This can be used for objects such as doors.
public class EnterAfterLife : MonoBehaviour, IInteractable
{
    public GameObject afterLife;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(){
        afterLife.SetActive(false);
    }
}
