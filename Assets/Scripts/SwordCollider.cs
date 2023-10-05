using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField]
    private string metaTag;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == metaTag)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}