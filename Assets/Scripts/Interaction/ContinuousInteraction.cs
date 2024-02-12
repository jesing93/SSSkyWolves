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
    protected override void Update()
    {
        base.Update();
        if (isBusy)
        {
            InteractionStay();
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public override void InteractionEnter()
    {
        isBusy = true;

        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                break;
            case InteractionType.GrabSmall:
                break;
            default: break;
        }

        
    }
    public void InteractionStay()
    {
        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                break;
            case InteractionType.GrabSmall:
                break;
            default: break;
        }
    }
    public override void InteractionExit()
    {
        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                break;
            case InteractionType.GrabSmall:
                break;
            default: break;
        }
        isBusy = false;
    }
    #endregion
}
