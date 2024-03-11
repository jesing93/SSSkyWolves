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
    public AudioSource buttonSound1;
    public AudioSource buttonSound2;

    [SerializeField]
    private AudioMixer audioMixer;

    private GameObject activePanel;
    private bool isLoadingSettings;
    [SerializeField]
    private GameObject[] settingsItems;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Start()
    {
        ApplyPrefs();

        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CambiarPanel(pausePanel);
            Time.timeScale = 0.0f;
        }
    }
    private void Awake()
    {
        Time.timeScale = 1.0f;
        LoadPrefs();
    }

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods


    public void OnClickContinue()
    {
        CambiarPanel();
        Time.timeScale = 1.0f;
        buttonSound1.Play();

    }
    public void OnClickOpciones()
    {
        buttonSound1.Play();
        CambiarPanel(optionPanel);
    }

    public void OnClickMenu()
    {
        buttonSound1.Play();
        SceneManager.LoadScene("MainTitle");
        Time.timeScale = 1.0f;
    }

    public void OnClickReiniciar()
    {
        buttonSound2.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void OnClickCreditos()
    {
        buttonSound1.Play();
        CambiarPanel(creditsPanel);
    }

    public void OnClickVolver()
    {
        buttonSound2.Play();
        CambiarPanel(pausePanel);
    }

    public void OnClickSalir()
    {
        buttonSound2.Play();
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
            PlayerPrefs.SetFloat("SFXUIVolume", settingsItems[2].GetComponent<Slider>().value);

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
            settingsItems[2].GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXUIVolume");

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
            audioMixer.SetFloat("SFXUIVolume", PlayerPrefs.GetFloat("SFXUIVolume"));
        }
    }
    #endregion
}