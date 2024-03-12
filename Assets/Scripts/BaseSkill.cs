using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    protected PlayerController controller;
    [SerializeField] protected int abilityIndex;
    public GameObject rcShootPoint;
    public LayerMask ignoreLayer;

    protected bool canUseSKill;

    [Header("Cooldown")]
    public float skillCd;
    [SerializeField] protected float skillCdTimer;




    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected virtual void Update()
    {
        if (skillCdTimer > 0)
            skillCdTimer -= Time.deltaTime;
    }
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public virtual void ActivateSkill()
    {
        canUseSKill = true;
        //Timer for cooldown
        if (skillCdTimer > 0) canUseSKill = false;
        else 
        { 
            skillCdTimer = skillCd;
            HUDController.instance.AbilityCooldownHUD(controller.isWhite, abilityIndex, skillCd);
        }
    }
    #endregion
}
