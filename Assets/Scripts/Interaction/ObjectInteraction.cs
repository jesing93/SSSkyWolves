using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
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
    protected void OnTriggerEnter(Collider other)
    {
    }

    protected void OnTriggerExit(Collider other)
    {
        //overrides base interaction stuff
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public void ToggleInteraction(bool toggle) => canInteract = toggle;

    public IEnumerator InteractionEnter(ObjectInteractionTrigger trigger)
    {
        transform.position = trigger.transform.position;
        transform.parent = trigger.transform;
        yield return new WaitForSeconds(0);
    }

    public IEnumerator InteractionExit() 
    {
        
        yield return new WaitForSeconds(0); 
    }
    #endregion
}
