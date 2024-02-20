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
    public DetectionLightType lightType;

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

    /// <summary>
    /// Check the light line of vision to the selected player
    /// </summary>
    /// <param name="player">Player to check</param>
    /// <returns></returns>
    public bool CheckLight(PlayerController player)
    {
        List<bool> hits = new();
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

    /// <summary>
    /// Throw a raycast to the players detection point to check line of sight
    /// </summary>
    /// <param name="detectionPoint">Target point to shoot</param>
    /// <param name="isWhite">Whish player we are checking</param>
    /// <returns></returns>
    private bool CheckDetetionPoint(Vector3 detectionPoint, bool isWhite)
    {

        if (lightType == DetectionLightType.Spot) //If spotlight
        {
            //If not in light cone
            if(Vector3.Angle(transform.forward, (detectionPoint - transform.position).normalized) > lightSource.spotAngle/2)
            {
                return false;
            }
        }
        else if (lightType == DetectionLightType.Directional) //If directional
        {
            Ray skyRay = new(detectionPoint, transform.position + (-lightSource.transform.forward * 100));
            Debug.DrawRay(detectionPoint, transform.position + (-lightSource.transform.forward * 100), Color.red, 1f);
            //Shoot ray to sky
            if (Physics.Raycast(skyRay, 100))
            {
                //If don't see sky
                return false;
            }
            Debug.DrawRay(transform.position + (-lightSource.transform.forward * 100),Vector3.down * 5, Color.blue, 1f);
            //If see sky
            return true;
        }

        Ray ray = new(transform.position, detectionPoint - transform.position);
        Debug.DrawRay(transform.position, detectionPoint - transform.position, Color.red, 1f);
        //Shoot ray to wolf
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

    #region Data
    public enum DetectionLightType
    {
        Point,
        Spot,
        Directional
    }
    #endregion
}
