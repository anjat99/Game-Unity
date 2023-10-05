using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    #region Data
    private Enemy enemy;
    private float idleTimer; 
    private float idleTrajanje; 
    #endregion
    public void Enter(Enemy enemy)
    {
        idleTrajanje = UnityEngine.Random.Range(1, 10);
        this.enemy = enemy;
    }

    public void Execute()
    {
        //Debug.Log("I'm idling");
        Idle();
        if(enemy.Meta != null)
        {
            enemy.PromeniStanje(new PatrolState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.tag == "Knife")
        {
            enemy.Meta = Player.Instance.gameObject;
        }
    }

    private void Idle()
    {
        enemy.MojAnimator.SetFloat("brzina", 0);
        idleTimer += Time.deltaTime;
        if(idleTimer >= idleTrajanje)
        {
            enemy.PromeniStanje(new PatrolState());
        }
    }
}