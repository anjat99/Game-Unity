using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    #region Data
    [SerializeField]
    private float brzina;
    private Rigidbody2D igrac;
    private Vector2 pravac; 
    #endregion
    void Start()
    {
        igrac = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        igrac.velocity = pravac * brzina;
    }

    public void Initialize(Vector2 pravac)
    {
        this.pravac = pravac;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}