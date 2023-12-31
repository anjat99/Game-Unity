﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    #region Data
    [SerializeField]
    private BoxCollider2D platformCollider;
    [SerializeField]
    private BoxCollider2D platformTrigger; 
    #endregion

    void Start()
    {
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player" || other.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(platformCollider, other, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(platformCollider, other, false);
        }
    }
}