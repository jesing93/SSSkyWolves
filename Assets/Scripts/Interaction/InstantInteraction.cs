using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class InstantInteraction : BaseInteraction
{
    //Region dedicated to the different Variables.
    #region Variables
    
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    //Method to start the interaction
    public override IEnumerator InteractionEnter(PlayerController player)
    {
        base.InteractionEnter(player);
        switch(interactionType)
        {
            case InteractionType.Sniff:
                break;
            default: break;
        }
        yield return StartCoroutine(base.InteractionEnter(player));
    }
    //Method to end the interaction
    public override IEnumerator InteractionExit()
    {
        switch (interactionType)
        {
            case InteractionType.Sniff:
                break;
            default: break;
        }
        base.InteractionExit();
        yield return StartCoroutine(base.InteractionExit());
    }
    #endregion
}
