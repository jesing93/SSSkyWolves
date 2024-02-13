using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    [SerializeField] private bool isOn;
    private Light lightSource;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters
    public bool IsOn { get => isOn; }

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Awake()
    {
        lightSource = GetComponentInChildren<Light>();
        lightSource.enabled = isOn;    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public void Switch()
    {
        isOn = !isOn;
        lightSource.enabled = isOn;
    }

    public bool CheckLight(bool isWhite)
    {
        if (isWhite)
        {
            return true;
        }
        else
        {
            return true;
        }
    }
    #endregion
}
