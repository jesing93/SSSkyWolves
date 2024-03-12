using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    public GameObject pausePanel;
    public GameObject optionPanel;
    public GameObject creditsPanel;
    public AudioSource audioSource;

    [SerializeField]
    private AudioMixer audioMixer;

    private GameObject activePanel;
    private bool isLoadingSettings;
    [SerializeField]
    private GameObject[] settingsItems;

    public static PauseMenu instance;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();
        LoadPrefs();
    }

    private void Start()
    {
        ApplyPrefs();

        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    public void PauseGame()
    {
        CambiarPanel(pausePanel);
    }

    public void ResumeGame()
    {
        CambiarPanel();
        audioSource.Play();
    }

    public void OnClickContinue()
    {
        GameManager.Instance.TogglePause();
    }
    public void OnClickOpciones()
    {
        audioSource.Play();
        CambiarPanel(optionPanel);
    }

    public void OnClickMenu()
    {
        audioSource.Play();
        SceneManager.LoadScene("MainTitle");
        Time.timeScale = 1.0f;
    }

    public void OnClickReiniciar()
    {
        audioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void OnClickCreditos()
    {
        audioSource.Play();
        CambiarPanel(creditsPanel);
    }

    public void OnClickVolver()
    {
        audioSource.Play();
        CambiarPanel(pausePanel);
    }

    public void OnClickSalir()
    {
        audioSource.Play();
        Application.Quit();
        Debug.Log("ME CIERRO A-");
    }

    void CambiarPanel(GameObject panel = null)
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if (panel != null)
        {
            panel.SetActive(true);
        }
    }
    public void OnSettingsChanged()
    {
        if (!isLoadingSettings)
        {
            //Save settings
            PlayerPrefs.SetFloat("MasterVolume", settingsItems[0].GetComponent<Slider>().value);
            PlayerPrefs.SetFloat("MusicVolume", settingsItems[1].GetComponent<Slider>().value);
            PlayerPrefs.SetFloat("SFXVolume", settingsItems[2].GetComponent<Slider>().value);

            //Applying to the audio mixer
            ApplyPrefs();
        }
    }

    private void LoadPrefs()
    {
        isLoadingSettings = true;
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            //Update UI
            settingsItems[0].GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
            settingsItems[1].GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
            settingsItems[2].GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXVolume");

        }
        isLoadingSettings = false;
    }

    private void ApplyPrefs()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            //Update audio mixer
            audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }
    }
    #endregion
}