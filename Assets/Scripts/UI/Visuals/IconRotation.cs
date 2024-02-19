using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IconRotation : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    [SerializeField] private float rotationSpeed;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Start()
    {
        transform.DORotate(new Vector3(0, 180, 0), rotationSpeed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    #endregion
}
