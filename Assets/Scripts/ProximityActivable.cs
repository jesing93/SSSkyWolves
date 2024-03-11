using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityActivable : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    [SerializeField] private ActivableType type;
    [SerializeField] private float activationDuration = -1;
    [SerializeField] private float deactivationDuration = -1;
    [SerializeField] private float activationDelay;
    [SerializeField] private float deactivationDelay;
    private Coroutine currentSequence;
    private bool isActivating;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    public ActivableType Type { get => type; }

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    /// <summary>
    /// Activate the activable item
    /// </summary>
    public void Activate()
    {
        if (currentSequence != null)
            StopCoroutine(currentSequence);
        isActivating = true;
        currentSequence = StartCoroutine(ExecuteSequence(activationDelay, activationDuration));
    }

    /// <summary>
    /// Deactivate the activable item
    /// </summary>
    public void Deactivate()
    {
        if (currentSequence != null)
            StopCoroutine(currentSequence);
        isActivating = false;
        currentSequence = StartCoroutine(ExecuteSequence(deactivationDelay, deactivationDuration));
    }

    /// <summary>
    /// Execute a sequence of actions
    /// </summary>
    private IEnumerator ExecuteSequence(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        if (isActivating)
            ActivationActions();
        else
            DeactivationActions();

        if (duration > -1)
        {
            yield return new WaitForSeconds(duration);
            if (isActivating)
                DeactivationActions();
            else
                ActivationActions();
        }
    }

    /// <summary>
    /// Execute the activation action that matches the own ActivableType
    /// </summary>
    private void ActivationActions()
    {
        switch (type)
        {
            case ActivableType.Fire:
                ActivateLight();
                break;
        }
    }

    /// <summary>
    /// Execute the deactivation action that matches the own ActivableType
    /// </summary>
    private void DeactivationActions()
    {
        switch (type)
        {
            case ActivableType.Fire:
                DeactivateLight();
                break;
        }
    }

    /////***** All type actions *****/////

    /// <summary>
    /// Activate a light source
    /// </summary>
    private void ActivateLight()
    {
        if(TryGetComponent<LightSource>(out LightSource lightSource))
        {
            if (!lightSource.IsOn)
                lightSource.Switch();
        }
    }

    /// <summary>
    /// Deactivate a light source
    /// </summary>
    private void DeactivateLight()
    {
        if (TryGetComponent<LightSource>(out LightSource lightSource))
        {
            if (lightSource.IsOn)
                lightSource.Switch();
        }
    }
    #endregion

    #region data

    /// <summary>
    /// The type of activable
    /// </summary>
    public enum ActivableType
    {
        Fire,
        Mechanism
    }

    #endregion
}