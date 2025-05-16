using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(1000);
        }
    }

}
