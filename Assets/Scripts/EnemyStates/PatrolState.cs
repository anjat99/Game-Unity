using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolTrajanje;
    public void Enter(Enemy enemy)
    {
        patrolTrajanje = UnityEngine.Random.Range(1, 10);
        
        this.enemy = enemy;
    }

    public void Execute()
    {
        //Debug.Log("Patroling");
        Patrol();
        enemy.Kretanje();
        if (enemy.Meta != null)
        {
            enemy.PromeniStanje(new RangedState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.tag == "Edge")
        {
            enemy.PromeniSmer();
        }
        if (other.tag == "Knife")
        {
            enemy.Meta = Player.Instance.gameObject;
        }
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolTrajanje)
        {
            enemy.PromeniStanje(new IdleState());
        }
    }
}