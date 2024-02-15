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
    /// <summary>
    /// Turn On/Off the light
    /// </summary>
    public void Switch()
    {
        isOn = !isOn;
        lightSource.enabled = isOn;
    }

    /// <summary>
    /// Turn On/Off the light to the desired value
    /// </summary>
    /// <param name="newState"></param>
    public void Switch(bool newState)
    {
        isOn = newState;
        lightSource.enabled = isOn;
    }

    public bool CheckLight(PlayerController player)
    {
        List<bool> hits = new();
        //TODO: Raycast checks
        //raycast first transform
        hits.Add(true);
        //raycast second transform
        hits.Add(false);

        if (hits[0] == true || hits[1] == true)
        {
            if (player.isWhite)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (player.isWhite)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    #endregion
}
