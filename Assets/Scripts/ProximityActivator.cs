using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ProximityActivable;

public class ProximityActivator : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    [SerializeField] private List<ActivableType> activatesEnter;
    [SerializeField] private List<ActivableType> activatesExit;
    [SerializeField] private List<ActivableType> deactivatesEnter;
    [SerializeField] private List<ActivableType> deactivatesExit;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<ProximityActivable>(out ProximityActivable activable))
        {
            if (activatesEnter.Contains(activable.Type)){
                activable.Activate();
            }
            else if (deactivatesEnter.Contains(activable.Type))
            {
                activable.Deactivate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<ProximityActivable>(out ProximityActivable activable))
        {
            if (activatesExit.Contains(activable.Type))
            {
                activable.Activate();
            }
            else if (deactivatesExit.Contains(activable.Type))
            {
                activable.Deactivate();
            }
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}