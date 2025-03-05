using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}
public class Interact : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    void Start()
    {

    }

    void Update()
    {
        // This code sends out a racast to an object and if it is interactable with then the player interacts with it.
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if(Physics.Raycast(r, out RaycastHit hitinfo, InteractRange))
            {
                if (hitinfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }



}
