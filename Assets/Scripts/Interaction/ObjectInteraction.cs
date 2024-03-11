using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    private bool canInteract;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected void OnTriggerEnter(Collider other)
    {
        //overrides base interaction stuff
    }

    protected void OnTriggerExit(Collider other)
    {
        //overrides base interaction stuff
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    //TODO: A Way to know when the item can interact and when it cant
    protected virtual IEnumerable OnInteractionEnter()
    {
        //TODO: turn on Fire When near a firePlace

        //TODO: snap To an Object when near them
        yield return new WaitForSeconds(0);
    }

    protected virtual IEnumerable OnInteractionExit() 
    {
        yield return new WaitForSeconds(0); 
    }
    #endregion
}
