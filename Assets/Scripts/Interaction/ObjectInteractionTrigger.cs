using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ObjectInteractionTrigger : BaseInteraction
{
    //Region dedicated to the different Variables.
    #region Variables

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected new void OnTriggerEnter(Collider other)
    {
        //TODO: Get Object Interactions in Range
        if (other.GetComponent<BaseInteraction>()?.GetType() == typeof(ObjectInteraction))
        {
            switch (interactionType)
            {
                case InteractionType.FireSource:
                    break;

                case InteractionType.ObjectSnapper: 
                    break;

                default: break;
            }
        }
    }

    protected new void OnTriggerExit(Collider other)
    {
        //TODO: Delete Object interactions in Range
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    protected IEnumerator InteractionEnter()
    {
        yield return new WaitForSeconds(0);
    }

    protected new IEnumerator InteractionExit()
    {

        yield return new WaitForSeconds(0);
    }
    #endregion
}
