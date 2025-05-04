using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IInteractable
{
    public GameObject dangerouspipe;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(){
        Debug.Log("Valve Interacted!");
        if (dangerouspipe != null)
        {
            BoxCollider collider = dangerouspipe.GetComponent<BoxCollider>();
            if (collider != null)
            {
                Destroy(collider);
            }
        }
    }
}

    
