using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    public GameObject[] gates;
    [SerializeField] GameObject gate1;
    [SerializeField] GameObject gate2;
    public Transform furthestGate;
  

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Start()
    {
        gate1 = this.transform.GetChild(0).gameObject;
        gate2 = this.transform.GetChild(1).gameObject;
        furthestGate = gates[0].transform;


    }
 

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    //Gets the furthest point of tp relative to the wolf 
    //DOESNT WORk
    public Transform GetFurthestGate(Transform wolf)
    {
        Debug.Log("Entra");
        if (Vector3.Distance(gates[0].transform.position, wolf.position) <= Vector3.Distance(gates[1].transform.position, wolf.position))
        {
            furthestGate = gates[1].transform;
            Debug.Log("funsiona");
        }
        return furthestGate;
    }
    #endregion
}
