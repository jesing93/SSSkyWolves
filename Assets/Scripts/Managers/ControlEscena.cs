using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class ControlEscena : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("HugoTesting");
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}
