using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IUseable
{
    [SerializeField]
    private Collider2D platformCollider;

    public void Use()
    {
        //Debug.Log("used ladder");
        if (Player.Instance.NaMerdevinama)
        {
            //prestati penjati se
            KoristiMerdevine(false, 1, 0, 1, "prizemljen");
        }
        else
        {
            //penjati se
            KoristiMerdevine(true, 0, 1, 0, "reset");
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, true);
        }
    }

    private void KoristiMerdevine(bool naMerdevinama, int gravity, int layerWeight, int brzinaAnim, string trigger)
    {
        Player.Instance.NaMerdevinama = naMerdevinama;
        Player.Instance.Igrac.gravityScale = gravity;
        Player.Instance.MojAnimator.SetLayerWeight(2, layerWeight);
        Player.Instance.MojAnimator.speed = brzinaAnim;
        Player.Instance.MojAnimator.SetTrigger(trigger);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            KoristiMerdevine(false, 1, 0, 1, "prizemljen");
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, false);
        }
    }
}