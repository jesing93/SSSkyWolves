using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseInteraction : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    [SerializeField] protected List<Transform> snapPoints;
    [SerializeField] protected List<Collider> colliders;
    [SerializeField] protected InteractionType interactionType;

    [SerializeField] protected bool canInteract;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            InteractionEnter();
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public abstract void InteractionEnter();
    public abstract void InteractionExit();
    #endregion

    //Region dedicated to related Data
    #region Data
    public enum InteractionType
    {
        Sniff,
        GrabSmall,
        GrabLarge,
    }
    #endregion
}
