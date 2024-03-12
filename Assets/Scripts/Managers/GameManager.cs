using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    [SerializeField] private GameObject whiteCamPref;
    [SerializeField] private GameObject blackCamPref;
    private List<LightSource> lightSources = new();
    private GameObject whiteSpawn;
    private GameObject blackSpawn;
    private float wolfsOnGoal = 0;
    private bool isGamePaused = false;


    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    public bool IsGamePaused { get => isGamePaused; }
    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Awake()
    {
        Instance = this;
        whiteSpawn = GameObject.FindGameObjectWithTag("WhiteSpawn");
        blackSpawn = GameObject.FindGameObjectWithTag("BlackSpawn");
        //Instantiate players
        white = Instantiate(whitePref, whiteSpawn.transform.position, whiteSpawn.transform.rotation).GetComponent<PlayerController>();
        black = Instantiate(blackPref, blackSpawn.transform.position, blackSpawn.transform.rotation).GetComponent<PlayerController>();
    }

    private void Start()
    {
        //Assign cameras
        CinemachineVirtualCamera whiteCam = whiteCamPref.GetComponentInChildren<CinemachineVirtualCamera>();
        CinemachineVirtualCamera blackCam = blackCamPref.GetComponentInChildren<CinemachineVirtualCamera>();
        whiteCam.Follow = white.transform;
        whiteCam.LookAt = white.cameraTrack;
        blackCam.Follow = black.transform;
        blackCam.LookAt = black.cameraTrack;
    }
    private void FixedUpdate()
    {
        CheckLights();
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    /// <summary>
    /// Respawn wolf on death
    /// </summary>
    /// <param name="isWhite"></param>
    public void WolfDeath(bool isWhite)
    {
        if (isWhite)
        {
            white.transform.SetPositionAndRotation(whiteSpawn.transform.position, whiteSpawn.transform.rotation);
            whiteSpawn.GetComponentInChildren<ParticleController>().PlaySystems();
        }
        else
        {
            black.transform.SetPositionAndRotation(blackSpawn.transform.position, blackSpawn.transform.rotation);
            blackSpawn.GetComponentInChildren<ParticleController>().PlaySystems();
        };
        //TODO: Lifes management
    }

    /// <summary>
    /// Toggle game pause
    /// </summary>
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        Debug.Log("Pause: " +isGamePaused);
        //Switch player inputs
        white.GetComponent<InputManager>().TogglePause(isGamePaused);
        black.GetComponent<InputManager>().TogglePause(isGamePaused);

        //Switch time scale
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            PauseMenu.instance.PauseGame();
        }
        else
        {
            Time.timeScale = 1f;
            PauseMenu.instance.ResumeGame();
        }
    }

    /// <summary>
    /// Wolf reached the goal of the level
    /// </summary>
    public void LevelGoalEnter()
    {
        wolfsOnGoal++;
        if (wolfsOnGoal > 1)
        {
            Win();
        }
    }

    /// <summary>
    /// Wolf exit the goal of the level
    /// </summary>
    public void LevelGoalExit()
    {
        wolfsOnGoal--;
    }

    /// <summary>
    /// Win event
    /// </summary>
    private void Win()
    {
        Debug.Log("Win!");
        //TODO: WinScreen
    }

    /// <summary>
    /// Add a light source to the list for light checks
    /// </summary>
    /// <param name="lightSource"></param>
    public void AddToLightSources(LightSource lightSource)
    {
        lightSources.Add(lightSource);
    }

    /// <summary>
    /// Check light exposure of each player
    /// </summary>
    private void CheckLights()
    {
        bool isWhiteHit = false;
        bool isBlackHit = false;
        foreach (LightSource lightSource in lightSources)
        {
            if (lightSource.IsOn)
            {
                if (!isWhiteHit && lightSource.CheckLight(white))
                {
                    isWhiteHit = true;
                }
                if (!isBlackHit && lightSource.CheckLight(black))
                {
                    isBlackHit = true;
                }
            }
        }
        white.ChangeLightExposition(isWhiteHit);
        black.ChangeLightExposition(isBlackHit);
    }
    #endregion
}
