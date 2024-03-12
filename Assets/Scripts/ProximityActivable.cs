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
    //private GameObject targetGO;

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
    public void Activate(/*GameObject tempTarget = null*/)
    {
        //targetGO = tempTarget;
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
            case ActivableType.Storable:
                ActivateStorable();
                break;
            case ActivableType.Droppable:
                ActivateDroppable();
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
            case ActivableType.Storable:
                DeactivateStorable();
                break;
            case ActivableType.Droppable:
                DeactivateDroppable();
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

    /// <summary>
    /// Set Storage Transform
    /// </summary>
    private void ActivateStorable()
    {
        if (TryGetComponent<ContinuousInteraction>(out ContinuousInteraction interaction))
        {
            //interaction.CurrentSnapPoint = targetGO.transform;
        }
    }

    /// <summary>
    /// Empty storage Transform
    /// </summary>
    private void DeactivateStorable()
    {
        if (TryGetComponent<ContinuousInteraction>(out ContinuousInteraction interaction))
        {
            interaction.CurrentSnapPoint = null;
        }
    }

    /// <summary>
    /// Set Storage Transform
    /// </summary>
    private void ActivateDroppable()
    {
        Debug.Log("Drop");
        if (TryGetComponent<ContinuousInteraction>(out ContinuousInteraction interaction))
        {
            Debug.Log("Continue");
            if (interaction.CurrentSnapPoint != null)
            {
                interaction.transform.parent = interaction.CurrentSnapPoint;
                interaction.transform.position = interaction.CurrentSnapPoint.position;
            }
            
        }
    }

    /// <summary>
    /// Empty storage Transform
    /// </summary>
    private void DeactivateDroppable() { }


    #endregion


}
#region data

/// <summary>
/// The type of activable
/// </summary>
public enum ActivableType
{
    Fire,
    Mechanism,
    Storable,
    Droppable
}

#endregion