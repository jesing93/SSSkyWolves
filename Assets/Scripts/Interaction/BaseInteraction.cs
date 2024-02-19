using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] protected List<Collider> triggers;
    [Tooltip("What action will the wolf do when interacting")]
    [SerializeField] protected InteractionType interactionType;


    [SerializeField] protected Animation enterAnimation;

    [SerializeField] protected Animation exitAnimation;

    [Tooltip("The Icon That Apears when getting close to the object")]
    [SerializeField] protected Image icon;

    protected PlayerController currentPlayer;

    protected bool canInteract;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    protected virtual void Awake()
    {
        icon = GetComponentInChildren<Image>(true);
        colliders = new();
        foreach (Collider collider in GetComponents<Collider>())
        {
            if (collider.isTrigger)
            {
                colliders.Add(collider);
            }
            else
            {
                triggers.Add(collider);
            }
        }

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddInteraction(this.transform);

            icon.enabled = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().RemoveInteraction(this.transform);

            icon.enabled = false;
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    //Method to start the interaction
    public virtual IEnumerator InteractionEnter(PlayerController player)
    {
        currentPlayer = player;

        icon.enabled = false;

        foreach (Collider collider in triggers)
        {
            collider.enabled = false;
        }

        currentPlayer.RemoveInteraction(this.transform);

        yield return new WaitForSeconds(/*enterAnimation.clip.length*/ 0);

        
    }

    //Method to end the interaction
    public virtual IEnumerator InteractionExit()
    {



        yield return new WaitForSeconds(/*exitAnimation.clip.length*/ 0 );

        foreach (Collider collider in triggers)
        {
            collider.enabled = true;
        }

        icon.enabled = false;

        currentPlayer = null;
    }
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
