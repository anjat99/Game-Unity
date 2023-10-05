using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;


public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    [SerializeField] AudioSource audioSrc;
    private float nVolume = 1f;
    public Dropdown ddlGraphics;
    Resolution[] rezolucije;
    public Dropdown rezolucijeDropdown;
    void Start()
    {
        //DontDestroyOnLoad(this);
        rezolucije = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        rezolucijeDropdown.ClearOptions();

        List<string> opcije = new List<string>();
        int trenutniIndexRezolucije = 0;
        for(int i=0; i < rezolucije.Length; i++)
        {
            string opcija = rezolucije[i].width + " x " + rezolucije[i].height;
            opcije.Add(opcija);

            if(rezolucije[i].width == Screen.currentResolution.width && rezolucije[i].height == Screen.currentResolution.height && Screen.currentResolution.refreshRate == rezolucije[i].refreshRate)
            {
                trenutniIndexRezolucije = i;
            }
        }

        rezolucijeDropdown.AddOptions(opcije);
        rezolucijeDropdown.value = trenutniIndexRezolucije;
        rezolucijeDropdown.RefreshShownValue();

        var quality = QualitySettings.GetQualityLevel();
        ddlGraphics.value = quality;
    }

    void Update()
    {
        audioSrc.volume = nVolume;
    }

    public void PodesiRezoluciju(int indexRezolucije)
    {
        Resolution rezolucija = rezolucije[indexRezolucije];
        Screen.SetResolution(rezolucija.width, rezolucija.height, Screen.fullScreen);
    }
    public void PodesiVolume(float volume)
    {
        //Debug.Log(volume);
        nVolume = volume;
        audioMixer.SetFloat("volume", nVolume);
        AudioListener.volume = nVolume;

    }

    public void PodesiQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void PodesiFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;  
    }

    public void BackToTheMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}