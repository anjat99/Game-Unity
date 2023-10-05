using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private int ostatakZivota = 3;

    [SerializeField]
    private GameObject coinPrefab;
    
    [SerializeField]
    private GameObject gemPrefab;

    [SerializeField]
    private Text coinText; 
    
    [SerializeField]
    private Text gemText;

    private int sakupljeniCoins = 0;
    private int sakupljeniGems = 0;

    void Start()
    {
        PlayerPrefs.SetInt("PlayerLivesTrenutno", ostatakZivota);

    }
    public static GameManager Instance 
    { 
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public GameObject CoinPrefab 
    { 
        get
        {
            return coinPrefab;
        }
    } 
    
    public GameObject GemPrefab 
    { 
        get
        {
            return gemPrefab;
        }
    }

    public int SakupljeniCoins 
    { 
        get
        {
            return sakupljeniCoins;
        }
        set
        {
            coinText.text = value.ToString();
            sakupljeniCoins = value;
        } 
    } 
  
    public int SakupljeniGems 
    { 
        get
        {
            return sakupljeniGems;
        }
        set
        {
            gemText.text = value.ToString();
            sakupljeniGems = value;
        } 
    }

    public int OstatakZivota
    {
        get
        {
            return ostatakZivota;
        }
        set
        {
            ostatakZivota = value;
        }
    }

   
}