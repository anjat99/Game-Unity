using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int levelIndex;
    public string levelName;
    

    public Image blackImg;
    public Animator anim;
    GameManager gm;

   void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Fading());
        }
    }

    IEnumerator Fading()
    {
        anim.SetBool("fade", true);
        yield return new WaitUntil(() => blackImg.color.a == 1);
        SceneManager.LoadScene(levelIndex);
        SceneManager.LoadScene(levelName);
        PlayerPrefs.SetInt("PlayerLivesTrenutno", gm.OstatakZivota);
    }
}