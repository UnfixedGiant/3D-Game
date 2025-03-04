using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
