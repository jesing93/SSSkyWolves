using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    [SerializeField] private bool isOn;
    private Light lightSource;
    [SerializeField] private float lightIntensity;

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
        lightSource.enabled = isOn;
        lightSource.shadowRadius = 0;
    }

    private void Start()
    {
        GameManager.Instance.AddToLightSources(this);
    }
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
        //raycast front transform
        hits.Add(CheckDetetionPoint(player.frontDetection.position, player.isWhite));
        //raycast back transform
        hits.Add(CheckDetetionPoint(player.backDetection.position, player.isWhite));
        if (hits[0] == true || hits[1] == true)
        {
            if(player.isWhite && (hits[0] != hits[1])){
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckDetetionPoint(Vector3 detectionPoint, bool isWhite)
    {
        Ray ray = new(transform.position, detectionPoint - transform.position);
        Debug.DrawRay(transform.position, detectionPoint - transform.position, Color.red, 1f);
        if (Physics.Raycast(ray, out RaycastHit hit, lightIntensity))
        {
            //If hit with the player
            if ((hit.collider.gameObject.CompareTag("White") && isWhite) || (hit.collider.gameObject.CompareTag("Black") && !isWhite))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
