using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private Enemy enemy;

    private float napadTimer;
    private float trajanjeMirovanjaPreSledecegNapada = 3;
    private bool mozeDaNapadne = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        //Debug.Log("I'm meeling");
        Napad();

        if(enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.PromeniStanje(new RangedState());
        }
        else if (enemy.Meta == null && enemy.InMeleeRange)
        {
            enemy.PromeniStanje(new IdleState());
        }

    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D other)
    {
       
    }

    private void Napad()
    {
        napadTimer += Time.deltaTime;

        if (napadTimer >= trajanjeMirovanjaPreSledecegNapada)
        {
            mozeDaNapadne = true;
            napadTimer = 0;
        }

        if (mozeDaNapadne)
        {
            mozeDaNapadne = false;
            enemy.MojAnimator.SetTrigger("napad");
        }
    }
}