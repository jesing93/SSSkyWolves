using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ObjectInteractionTrigger : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected  void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent<ObjectInteraction>(out ObjectInteraction interactedObject))
        {
            StartCoroutine(interactedObject.InteractionEnter(this));
        }

    }

    protected  void OnTriggerExit(Collider other)
    {
        //TODO: Delete Object interactions in Range
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    #endregion
}
