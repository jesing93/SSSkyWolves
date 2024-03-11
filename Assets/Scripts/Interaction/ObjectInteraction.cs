using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : BaseInteraction
{
    //Region dedicated to the different Variables.
    #region Variables
    private bool canInteract;
    private GameObject interactableTrigger;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected new void OnTriggerEnter(Collider other)
    {
        //overrides base interaction stuff
    }

    protected new void OnTriggerExit(Collider other)
    {
        //overrides base interaction stuff
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    //TODO: A Way to know when the item can interact and when it cant
    protected IEnumerator InteractionEnter()
    {
        //TODO: turn on Fire When near a firePlace

        //overrides base interaction stuff
        /*transform.position = other.transform.position;*/
        yield return new WaitForSeconds(0);
    }

    protected new IEnumerator InteractionExit() 
    {
        yield return new WaitForSeconds(0); 
    }
    #endregion
}
