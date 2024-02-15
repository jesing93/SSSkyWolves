using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousInteraction : BaseInteraction
{
    //Region dedicated to the different Variables.
    #region Variables
    private bool isBusy;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected void Update()
    {
        if (isBusy)
        {
            InteractionStay();
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    //Method to start the interaction
    public override IEnumerator InteractionEnter(PlayerController player)
    {
        yield return StartCoroutine(base.InteractionEnter(player));
        isBusy = true;

        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                //TODO: Make the object the parent of the wolf, limiting its movement
                break;
            case (InteractionType.GrabSmall):
                //TODO: Make the wolf the parent of the wolf, letting it move freely
                break;
            default: break;
        }
        Debug.Log("I Reached here");
    }
    //Method repeated whilst the interaction takes place
    public void InteractionStay()
    {
        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                //TODO: Detects wether to move or not 
                break;
            case InteractionType.GrabSmall:
                //TODO: Not sure
                break;
            default: break;
        }
    }
    //Method to end the interaction
    public override IEnumerator InteractionExit()
    {
        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                //TODO: Unparents the objects 
                break;
            case InteractionType.GrabSmall:
                //TODO: Unparents the objects
                break;
            default: break;
        }

        base.InteractionExit();
        isBusy = false;
        yield return StartCoroutine(base.InteractionExit());
    }
    #endregion
}
