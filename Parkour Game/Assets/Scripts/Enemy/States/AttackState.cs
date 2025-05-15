using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float shotTimer;

    private float moveTimer;
    private float losePlayerTimer;

    private bool useFirstBarrel = true;

    public override void Enter()
    {

    }
    public override void Exit()
    {

    }



    public override void Perform()
    {
        // Checking to see if the player can be seen.
        if (enemy.CanSeePlayer())
        {
            enemy.EnemyAnim.SetBool("isWalking", false);
            enemy.EnemyAnim.SetBool("isIdle", false);
            enemy.EnemyAnim.SetBool("isAttacking", true);
            // Incrementing timers for moving and shooting.
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }
            // Moving the enemy to a random position after a random time.
            if(moveTimer > Random.Range(3,7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.changeState(new PatrolState());
                enemy.EnemyAnim.SetBool("isAttacking", false);
            }
        }
    }

    public void Shoot()
    {
        Transform gunBarrel1 = enemy.gunBarrel1;
        Transform gunBarrel2 = enemy.gunBarrel2;
        // Change between barrels since enemy has 2 weapons.
        Transform selectedBarrel = useFirstBarrel ? gunBarrel1 : gunBarrel2;

        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, selectedBarrel.position, enemy.transform.rotation);

        Vector3 shootDirection = (enemy.Player.transform.position - selectedBarrel.transform.position).normalized;
        // Adding force to the bullet rigidbody.
        // Using Quaternion to add randomness to where the bullet travels to make sure it doesnt hit the player every time.
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 90;

        shotTimer = 0;
        // Reset barrel after enemy has shot.
        useFirstBarrel = !useFirstBarrel;
    }

















    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
