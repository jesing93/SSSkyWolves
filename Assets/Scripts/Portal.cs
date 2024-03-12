using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    public GameObject[] gates;
  

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    //Gets the furthest point of tp relative to the wolf 
    //DOESNT WORk
    public Transform GetFurthestGate(Transform wolf)
    {
        Transform furthestGate;
        Debug.Log("Entra");
        if (Vector3.Distance(gates[0].transform.position, wolf.position) <= Vector3.Distance(gates[1].transform.position, wolf.position))
        {
            furthestGate = gates[1].transform;
            Debug.Log("Gate1");
        }
        else
        {
            furthestGate = gates[0].transform;
            Debug.Log("Gate0");
        }
        return furthestGate;
    }
    #endregion
}
