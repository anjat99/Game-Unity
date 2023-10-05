using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    #region Data
    private IEnemyState trenutnoStanje;
    public GameObject Meta { get; set; }

    [SerializeField]
    private float meleeRange;
    
    [SerializeField]
    private float throwRange;

    private Vector2 startPosition;

    [SerializeField]
    private Transform levaIvica;

    [SerializeField]
    private Transform desnaIvica;

    private Canvas healthCanvas;

    //Coin - enemy
    private bool dropCoin = true;

    [SerializeField]
    private AudioSource deadEnemySound;
    public bool InMeleeRange
    {
        get
        {
            if( Meta != null)
            {
                return Vector2.Distance(transform.position, Meta.transform.position) <= meleeRange;
            }

            return false;
        }
    } 
    
    public bool InThrowRange
    {
        get
        {
            if( Meta != null)
            {
                return Vector2.Distance(transform.position, Meta.transform.position) <= throwRange;
            }

            return false;
        }
    }

    public override bool DaliJeMrtav
    {
        get
        {
            return healthStat.TrenutnaVrednost <= 0;

        }
    }

    #endregion

    public override void Start()
    {
        base.Start();
        startPosition = transform.position;
        Player.Instance.Dead += new DeadEventHandler(UkloniMetu);
        PromeniStanje(new IdleState());

        healthCanvas = transform.GetComponentInChildren<Canvas>();
    }

    void Update()
    {
        if (!DaliJeMrtav)
        {
            if (!TakingDamage)
            {
                trenutnoStanje.Execute();
            }

            PogledajUMetu();
        }
    }

    public void UkloniMetu()
    {
        Meta = null;
        PromeniStanje(new PatrolState());
    }

    private void PogledajUMetu()
    {
        if (Meta != null)
        {
            float xPravac = Meta.transform.position.x - transform.position.x;
            if (xPravac < 0 && praviSmer || xPravac > 0 && !praviSmer)
            {
                PromeniSmer();
            }
        }
    }

    public void PromeniStanje(IEnemyState novoStanje)
    {
        if(trenutnoStanje != null)
        {
            trenutnoStanje.Exit();
        }
        trenutnoStanje = novoStanje;
        trenutnoStanje.Enter(this);
    }

    public void Kretanje()
    {
        if (!Napad)
        {
            if ((DohvatiSmer().x > 0 && transform.position.x < desnaIvica.position.x) || (DohvatiSmer().x < 0 && transform.position.x > levaIvica.position.x))
            {
                MojAnimator.SetFloat("brzina", 1);
                transform.Translate(DohvatiSmer() * (brzinaKretanja * Time.deltaTime));
            }
            else if (trenutnoStanje is PatrolState)
            {
                PromeniSmer();
            }
            else if(trenutnoStanje is RangedState)
            {
                Meta = null;
                PromeniStanje(new IdleState());
            }
        }
    }

    public Vector2 DohvatiSmer()
    {
        return praviSmer ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("hit");
        base.OnTriggerEnter2D(other);
        trenutnoStanje.OnTriggerEnter(other);
    }
    
    public override IEnumerator TakeDamage()
    {
        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }

        healthStat.TrenutnaVrednost -= 10;

        if (!DaliJeMrtav)
        {
            MojAnimator.SetTrigger("damage");
            //Debug.Log("osetecen protivnik");

        }
        else
        {
            //Coins
            if (dropCoin)
            {
                GameObject coin = Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
              
                Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                
                dropCoin = false;
            }

            MojAnimator.SetTrigger("die");
            //Debug.Log("mrtav protivnik");
            deadEnemySound.Play();

            yield return null;
        }
    }

    public override void Death()
    {
        //Coins
        dropCoin = true;

        Destroy(gameObject);

        healthCanvas.enabled = false;
    }

    public override void PromeniSmer()
    {
        //Referenca na canvas enemy-ja
        Transform nadjiChild = transform.Find("EnemyHealthBarCanvas").transform;
        Vector3 pozicija = nadjiChild.position;
        nadjiChild.SetParent(null);

        ///promena smera enemy-ja
        base.PromeniSmer();

        //Vracanje health bara kod enemy-ja
        nadjiChild.SetParent(transform);

        //Vracanje health bara u korektnu poziciju 
        nadjiChild.position = pozicija;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      
        if (other.gameObject.CompareTag("Gem"))
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
        }
    }
}