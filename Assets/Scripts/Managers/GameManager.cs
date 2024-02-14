using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    //Singleton
    public static GameManager Instance;

    //Components
    private PlayerController white;
    private PlayerController black;
    [SerializeField] private GameObject whitePref;
    [SerializeField] private GameObject blackPref;
    private List<LightSource> lightSources;
    private GameObject whiteSpawn;
    private GameObject blackSpawn;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Awake()
    {
        Instance = this;
        whiteSpawn = GameObject.FindGameObjectWithTag("WhiteSpawn");
        blackSpawn = GameObject.FindGameObjectWithTag("BlackSpawn");

        //TODO: Instantiate players
        //white = Instantiate(whitePref, whiteSpawn.transform.position, whiteSpawn.transform.rotation).GetComponent<PlayerController>();
        //black = Instantiate(blackPref, blackSpawn.transform.position, blackSpawn.transform.rotation).GetComponent<PlayerController>();
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
