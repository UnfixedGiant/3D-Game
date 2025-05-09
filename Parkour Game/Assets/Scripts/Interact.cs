using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interact : MonoBehaviour
{
    private Camera cam;
    public float InteractRange;
    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private PlayerUI playerUI;

    [Header("Keybinds")]
    public KeyCode interactKey = KeyCode.E;

    void Start()
    {
        cam = GetComponent<Camera>();
        playerUI = GetComponent<PlayerUI>();
    }



    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray r = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(r.origin, r.direction * InteractRange);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null);
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (Input.GetKeyDown(interactKey))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }

}

        // This code sends out a raycast to an object and if it is interactable then the player interacts with it.
        
            // Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            // if(Physics.Raycast(r, out RaycastHit hitinfo, InteractRange, mask))
            // {
            //     Debug.Log(mask);
            //     if (Input.GetKeyDown(KeyCode.E))
            //     {
            //         if (hitinfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            //         {
            //             interactObj.Interact();
            //         }

            //     }       

            // }