using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class ControlEscena : MonoBehaviour
{

    //Region dedicated to the different Variables.
    #region Variables
    enum Type { AnyKey, Countdown }

    [SerializeField] private Type type;
    [SerializeField] private float countdown;
    [SerializeField] private string sceneName;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Update()
    {
        if (type == Type.AnyKey && Input.anyKeyDown)
        {
            LoadScene();
        }
        if(type == Type.Countdown)
        {
            if (countdown > 0) countdown -= Time.deltaTime;
            else LoadScene();

            if(Input.GetKeyDown(KeyCode.LeftControl)) LoadScene();
        }
    }
    void LoadScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}
