using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("White"))
        {
            TriggerDamage(true);
        }
        else if (other.CompareTag("Black"))
        {
            TriggerDamage(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerDamage(true);
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    private void TriggerDamage(bool isWhite)
    {
        GameManager.Instance.WolfDeath(isWhite);
    }
    #endregion
}
