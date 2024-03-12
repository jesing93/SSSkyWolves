using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    [SerializeField]private bool isEndOfLevel;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Black") || other.CompareTag("White"))
        {
            other.GetComponent<PlayerController>().ProtectedArea(true);
            if(isEndOfLevel) 
                GameManager.Instance.LevelGoalEnter(other.GetComponent<PlayerController>().isWhite);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Black") || other.CompareTag("White"))
        {
            other.GetComponent<PlayerController>().ProtectedArea(false);
            if (isEndOfLevel)
                GameManager.Instance.LevelGoalExit(other.GetComponent<PlayerController>().isWhite);
        }
    }

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}