using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseInteraction : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    [Tooltip("Where the wolf will snap to in order to interact")]
    [SerializeField] protected List<Transform> snapPoints;
    [Tooltip("If the wolf needs to face a specific direction before interacting")]
    [SerializeField] protected bool hasOrientation;
    [Tooltip("All the colliders associated with the Interactable object")]
    [SerializeField] protected List<Collider> colliders;
    [Tooltip("What action will the wolf do when interacting")]
    [SerializeField] protected InteractionType interactionType;

    protected bool canInteract;

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

    //Method to start the interaction
    public abstract void InteractionEnter();

    //Method to end the interaction
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
