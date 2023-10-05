using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxVrednost;

    [SerializeField]
    private float trenutnaVrednost;


    public float TrenutnaVrednost 
    {
        get
        {
            return trenutnaVrednost;
        }
        set
        {
            trenutnaVrednost = Mathf.Clamp(value, 0, MaxVrednost);
            bar.Vrednost = trenutnaVrednost;
        }
    }

    public float MaxVrednost 
    {
        get
        {
            return maxVrednost;
        }
        set
        {
            maxVrednost = value;
            bar.MaxVrednost = maxVrednost;
        }
    }

    public void Initialize()
    {
        this.MaxVrednost = maxVrednost;
        this.TrenutnaVrednost = trenutnaVrednost;
    }
}