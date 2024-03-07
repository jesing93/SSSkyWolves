using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    public GameObject pausePanel;
    public GameObject optionPanel;
    public GameObject creditsPanel;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Start()
    {
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

    public void OnClickContinue()
    {
        CambiarPanel();
        Time.timeScale = 1.0f;
    }
    public void OnClickOpciones()
    {
        CambiarPanel(optionPanel);
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("MainTitle");
        Time.timeScale = 1.0f;
    } 
    
    public void OnClickReiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void OnClickCreditos()
    {
        CambiarPanel(creditsPanel);
    }

    public void OnClickVolver()
    {
        CambiarPanel(pausePanel);
    }

    public void OnClickSalir()
    {
        Application.Quit();
        Debug.Log("ME CIERRO A-");
    }

    void CambiarPanel(GameObject panel = null)
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if(panel != null)
        { 
            panel.SetActive(true);
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}
