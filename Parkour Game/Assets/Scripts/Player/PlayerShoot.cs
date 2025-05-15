using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Gun")]
    public Transform gunBarrel;

    [Header("Shooting")]
    public float fireRate = 0.3f;
    private float shotTimer = 0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && shotTimer > fireRate)
        {
            Shoot();
            shotTimer = 0;
        }
    }


private void Shoot()
{
    GameObject bullet = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, transform.rotation);

    // Get the main camera
    Camera cam = Camera.main;
    Vector3 shootDirection = cam.transform.forward;

    // Raycast from the center of the screen
    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 1000f))
    {
        // Shoot towards the hit point
        shootDirection = (hit.point - gunBarrel.position).normalized;
    }
    else
    {
        // Shoot straight forward from the camera
        shootDirection = ray.direction;
    }

    bullet.GetComponent<Rigidbody>().velocity = shootDirection * 90;
}
}
