using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    #region Data
    [SerializeField]
    protected float brzinaKretanja;
    protected bool praviSmer;

    [SerializeField]
    private GameObject knifePrefab;

    [SerializeField]
    protected Stat healthStat;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    [SerializeField]
    private List<string> damageSources;

    public abstract bool DaliJeMrtav { get; }
    public bool TakingDamage { get; set; }
    public Animator MojAnimator { get; private set; }
    public bool Napad { get; set; }
    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    #endregion

    public virtual void Start()
    {
        //Debug.Log("Character start");

        praviSmer = true;
        MojAnimator = GetComponent<Animator>();
        healthStat.Initialize();
    }

    public abstract IEnumerator TakeDamage();
    public abstract void Death();
    public virtual void PromeniSmer()
    {
        //Debug.Log("Promena smera");
        praviSmer = !praviSmer;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void BacanjeNozeva(int vrednost)
    {
        if (praviSmer)
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public void MeleeNapad()
    {
        SwordCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}