using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

   
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowMainMenu()
    {
        Debug.Log("Main menu");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}