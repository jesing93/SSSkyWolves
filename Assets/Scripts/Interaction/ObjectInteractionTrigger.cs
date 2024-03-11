using System.Collections;
using System.Collections.Generic;
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
    protected void OnTriggerEnter(Collider other)
    {
        //TODO: Get Object Interactions in Range
    }

    protected void OnTriggerExit(Collider other)
    {
        //TODO: Delete Object interactions in Range
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    protected virtual IEnumerable OnInteractionEnter()
    {
        yield return new WaitForSeconds(0);
    }

    protected virtual IEnumerable OnInteractionExit()
    {
        yield return new WaitForSeconds(0);
    }
    #endregion
}
