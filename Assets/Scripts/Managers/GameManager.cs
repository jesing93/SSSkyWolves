using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    public static GameManager Instance;
    private List<LightSource> lightSources;
    private PlayerController white;
    private PlayerController black;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        CheckLights();
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public void WolfDeath()
    {
        //TODO: Lifes management
    }

    public void OnEndLevelEnter()
    {
        //TODO: Detect when a wolf reaches the end of the level
    }

    public void OnEndLevelExit()
    {
        //TODO: Detect when a wolf leaves the end of the level
    }

    public void AddToLightSources(LightSource lightSource)
    {
        lightSources.Add(lightSource);
    }

    private void CheckLights()
    {
        bool isWhiteHit = false;
        bool isBlackHit = false;
        foreach (LightSource lightSource in lightSources)
        {
            if (lightSource.IsOn)
            {
                if (!isWhiteHit && lightSource.CheckLight(true))
                {
                    isWhiteHit = true;
                }
                if (lightSource.CheckLight(false))
                {
                    isBlackHit = true;
                }
            }
        }
        white.IsInLight = isWhiteHit;
        black.IsInLight = isBlackHit;
    }
    #endregion
}