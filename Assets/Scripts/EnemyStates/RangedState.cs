using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;

    private float bacanjeNozaTimer;
    private float trajanjeMirovanjaPreBacanjaSledecegNoza = 3;
    private bool mozeDaBaciNoz = true;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        //Debug.Log("I'm ranged");
        BacanjeNozeva();

        if (enemy.InMeleeRange)
        {
            enemy.PromeniStanje(new MeleeState());
        }
        else if (enemy.Meta != null)
        {
            enemy.Kretanje();
        }
        else
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

    private void BacanjeNozeva()
    {
        bacanjeNozaTimer += Time.deltaTime;

        if(bacanjeNozaTimer >= trajanjeMirovanjaPreBacanjaSledecegNoza)
        {
            mozeDaBaciNoz = true;
            bacanjeNozaTimer = 0;
        }

        if (mozeDaBaciNoz)
        {
            mozeDaBaciNoz = false;
            enemy.MojAnimator.SetTrigger("bacanje");
        }
    }
}