using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    private Image pausePanel;
    private Image optionPanel;
    private Image creditsPanel;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Start()
    {
        pausePanel.enabled = false;
        optionPanel.enabled = false;
        creditsPanel.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.enabled = true;
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}
