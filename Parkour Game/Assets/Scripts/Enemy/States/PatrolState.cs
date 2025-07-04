using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public float waitTimer;
    public int waypointIndex;
    public override void Enter()
    {

    }
    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.changeState(new AttackState());
            return;
        }
        PatrolCycle();
    }
    public override void Exit()
    {
        
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            enemy.EnemyAnim.SetBool("isWalking", false);
            enemy.EnemyAnim.SetBool("isIdle", true);
            if (waitTimer > 3)
            {

                if (waypointIndex < enemy.path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                }
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
                enemy.EnemyAnim.SetBool("isWalking", true);
                enemy.EnemyAnim.SetBool("isIdle", false);
            }
        }
    }
}
