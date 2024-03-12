using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    private List<ParticleSystem> pSystems = new();
    #endregion

    //Region dedicated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    private void Awake()
    {
        pSystems.Add(GetComponent<ParticleSystem>());
        foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>())
            pSystems.Add(system);
    }

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    /// <summary>
    /// Play all the particle systems
    /// </summary>
    public void PlaySystems()
    {
        foreach (ParticleSystem system in pSystems)
            system.Play();
    }

    /// <summary>
    /// Play all the particle systems
    /// </summary>
    public void PlaySystems(float delay)
    {
        foreach (ParticleSystem system in pSystems)
            system.Play();
        StartCoroutine(StopDelay(delay));
    }

    /// <summary>
    /// Stop particle systems after the delay
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator StopDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopSystems();
    }

    /// <summary>
    /// Stop all the particle systems
    /// </summary>
    public void StopSystems()
    {
        foreach (ParticleSystem system in pSystems)
            system.Stop();
    }

    #endregion
}