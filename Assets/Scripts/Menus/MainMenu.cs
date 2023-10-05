using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

    public void ShowTheControls()
    {
        SceneManager.LoadScene("Controls");
    }
    
    public void ShowTheOptions()
    {
        SceneManager.LoadScene("Options");
    }
    public void BackToTheMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
 
}