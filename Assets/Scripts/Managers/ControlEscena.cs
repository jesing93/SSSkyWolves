using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ControlEscena : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    //public PlayerInput input;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Awake()
    {
        //input = new();
        //input += ctx => LoadScene();
        InputSystem.onAnyButtonPress.Call(ctx => LoadScene());
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
