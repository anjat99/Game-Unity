using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour
{
    private Text livesText;
    void Awake()
    {
        livesText = GetComponent<Text>();
        livesText.text = "LIVES: 3";
    }

    void Update()
    {
        livesText.text = "LIVES: " + GameManager.Instance.OstatakZivota.ToString();
    }
}