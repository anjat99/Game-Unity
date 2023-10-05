using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void DeadEventHandler();

public class Player : Character
{
    #region Data
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    private Vector2 startPosition;

    public event DeadEventHandler Dead;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask tipTla;

    [SerializeField]
    private float snagaSkoka;

    [SerializeField]
    private bool kontrolaUVazduhu;

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    [SerializeField]
    private float brzinaPenjanja;

    private IUseable useable;

    private SpriteRenderer spriteRenderer;

    //Sounds
    [SerializeField]
    private AudioSource zvukBacanjaNozeva;

    [SerializeField]
    private AudioSource zvukUdarcaMacem;

    [SerializeField]
    private AudioSource zvukSkoka;

    [SerializeField]
    private AudioSource zvukUklizavanja;

    [SerializeField]
    private AudioSource damageSound;

    [SerializeField]
    private AudioSource sakupljeniNovcic;

    [SerializeField]
    private AudioSource zvukNaMerdevinama;

    [SerializeField]
    private AudioSource deadPlayerSound;

    //Properties
    public Rigidbody2D Igrac { get; set; }
    public bool NaMerdevinama { get; set; }
    public bool Uklizavanje { get; set; }
    public bool Skok { get; set; }
    public bool NaTlu { get; set; }
    public override bool DaliJeMrtav
    {
        get
        {
            if (healthStat.TrenutnaVrednost <= 0)
            {
                OnDead();
            }
            return healthStat.TrenutnaVrednost <= 0;
        }
    }

    #endregion

    public override void Start()
    {
        //Debug.Log("Player start");
        base.Start();
        NaMerdevinama = false;
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Igrac = GetComponent<Rigidbody2D>();

        //GameManager.OstatakZivota = 3;
       
        if(Application.loadedLevelName == "Level2")
        {
            GameManager.Instance.OstatakZivota = PlayerPrefs.GetInt("PlayerLivesTrenutno");
            //GameManager.Instance.SakupljeniCoins = PlayerPrefs.GetInt("SakupljeniNovcici");
            PlayerPrefs.SetInt("SakupljeniNovcici", GameManager.Instance.SakupljeniCoins);

            GameManager.Instance.SakupljeniGems = PlayerPrefs.GetInt("SakupljeniGems");
        }

    }

    void Update()
    {
        if (!TakingDamage && !DaliJeMrtav)
        {
            if (transform.position.y <= -14f)
            {
                Death();
            }
            ManipulisiInputom();
        }
    }

    void FixedUpdate()
    {
        if (!TakingDamage && !DaliJeMrtav)
        {
            float horizontalnoKretanje = Input.GetAxis("Horizontal");
            float vertikalnoKretanje = Input.GetAxis("Vertical");

            NaTlu = DaLiJeIgracNaTlu();
            ManipulisiKretanje(horizontalnoKretanje, vertikalnoKretanje);
            FlipujSmer(horizontalnoKretanje);
            ManipulisiLayerima();
        }
    }

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    //Zvukovi
    private void ZvukBacanjaNozeva()
    {
        zvukBacanjaNozeva.Play();
    }
    private void ZvukUdarcaMacem()
    {
        zvukUdarcaMacem.Play();
    }
    private void ZvukSkakanja()
    {
        zvukSkoka.Play();
    }
    private void ZvukUklizavanja()
    {
        zvukUklizavanja.Play();
    }
    private void ZvukNaMerdevinama()
    {
        zvukNaMerdevinama.Play();
    }

    //f-ja za manipulisanjem kretanja igraca
    private void ManipulisiKretanje(float horizontalnoKretanje, float vertikalnoKretanje)
    {
      
        if (Igrac.velocity.y < 0)
        {
            MojAnimator.SetBool("prizemljen", true);
        }
        if(!Napad && !Uklizavanje && (NaTlu || kontrolaUVazduhu))
        {
            Igrac.velocity = new Vector2(horizontalnoKretanje * brzinaKretanja, Igrac.velocity.y);

        }
        if (Skok && Igrac.velocity.y == 0 && !NaMerdevinama) 
        {
            Igrac.AddForce(new Vector2(0, snagaSkoka));
        }
        if (NaMerdevinama)
        {
            MojAnimator.speed = vertikalnoKretanje != 0 ? Mathf.Abs(vertikalnoKretanje) : Mathf.Abs(horizontalnoKretanje);
            Igrac.velocity = new Vector2(horizontalnoKretanje * brzinaPenjanja, vertikalnoKretanje * brzinaPenjanja);
        }
        MojAnimator.SetFloat("brzina", Mathf.Abs(horizontalnoKretanje));
    }

    private void ManipulisiInputom()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !NaMerdevinama )
        {
            MojAnimator.SetTrigger("skok");
            ZvukSkakanja();
        }  
        if (Input.GetKeyDown(KeyCode.X)) //left shift 
        {
            MojAnimator.SetTrigger("napad");
            ZvukUdarcaMacem();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MojAnimator.SetTrigger("uklizavanje");
            ZvukUklizavanja();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            MojAnimator.SetTrigger("bacanje");
            ZvukBacanjaNozeva();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
            ZvukNaMerdevinama();
        }
    }

    //f-ja za promenu smera kretanja igraca
    private void FlipujSmer(float horizontalnoKretanje)
    {
        if(horizontalnoKretanje > 0 && !praviSmer || horizontalnoKretanje < 0 && praviSmer)
        {
            PromeniSmer();
        }
    }

    private bool DaLiJeIgracNaTlu()
    {
        if(Igrac.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] sviColliders = Physics2D.OverlapCircleAll(point.position, groundRadius, tipTla);

                for (int i = 0; i < sviColliders.Length; i++)
                {
                    if(sviColliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void ManipulisiLayerima()
    {
        if (!NaTlu)
        {
            MojAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MojAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void BacanjeNozeva(int vrednost)
    {
        if (!NaTlu && vrednost == 1 || NaTlu && vrednost == 0)
        {
            base.BacanjeNozeva(vrednost);
        }
    }

    private IEnumerator OznaciImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            healthStat.TrenutnaVrednost -= 10;

            damageSound.Play();
            if (!DaliJeMrtav)
            {
                //Debug.Log("ostecenje igraca");
                MojAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(OznaciImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MojAnimator.SetLayerWeight(1, 0);
                MojAnimator.SetTrigger("die");
                //Debug.Log("mrtav igraca");

            }
        }
    }

    public override void Death()
    {
        deadPlayerSound.Play();

        GameManager.Instance.OstatakZivota--;

        PlayerPrefs.SetInt("PlayerLivesTrenutno", GameManager.Instance.OstatakZivota);



        if (GameManager.Instance.OstatakZivota <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        Igrac.velocity = Vector2.zero;
        MojAnimator.SetTrigger("idle");
        transform.position = startPosition;
    }

    private void Use()
    {
        if(useable != null)
        {
            useable.Use();
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Useable"))
        {
            useable = other.GetComponent<IUseable>();
        }

        base.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    { 
        if(other.CompareTag("Useable"))
        {
            useable = null;
        }
    }

    //Coins
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            sakupljeniNovcic.Play();
            GameManager.Instance.SakupljeniCoins++;

            Destroy(other.gameObject);
            //PlayerPrefs.SetInt("SakupljeniNovcici", GameManager.Instance.SakupljeniCoins);

        }
        else if(other.gameObject.CompareTag("Gem"))
        {
            sakupljeniNovcic.Play();
            GameManager.Instance.SakupljeniGems++;
            PlayerPrefs.SetInt("SakupljeniGems", GameManager.Instance.SakupljeniGems);

            Destroy(other.gameObject);
        }
    }
}