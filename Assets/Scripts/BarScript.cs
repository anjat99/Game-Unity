using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    //[SerializeField]
    private float kolicinaPopuneBoje;

    [SerializeField]
    private Image sadrzaj;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private bool lerpColors;

    public float MaxVrednost { get; set; }
    public float Vrednost 
    {
        set
        {
            string[] niz = valueText.text.Split(':');
            valueText.text = niz[0] + ": " + value;
            kolicinaPopuneBoje = Map(value, 0, MaxVrednost, 0, 1);
        }
    }

    void Start()
    {
        if (lerpColors)
        {
            sadrzaj.color = fullColor;
        }


    }

    void Update()
    {
        UpravljajBarom();
    }

    private void UpravljajBarom()
    {
        if(kolicinaPopuneBoje != sadrzaj.fillAmount)
        {
            sadrzaj.fillAmount = Mathf.Lerp(sadrzaj.fillAmount, kolicinaPopuneBoje, Time.deltaTime * lerpSpeed);
        }
        if (lerpColors)
        {
            sadrzaj.color = Color.Lerp(lowColor, fullColor, kolicinaPopuneBoje);
        }
    }

    private float Map(float vrednostHealth, float inMin, float inMax, float outMin, float outMax)
    {
        return (vrednostHealth - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}