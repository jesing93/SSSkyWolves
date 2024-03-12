using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    protected PlayerController controller;
<<<<<<< Updated upstream
    public GameObject wolf;
=======
    [SerializeField] protected int skillIndex;
>>>>>>> Stashed changes
    public GameObject rcShootPoint;
    public LayerMask ignoreLayer;
    protected bool canUseSkill;

    [Header("KeyBinds")]
    public KeyCode dashkey = KeyCode.LeftShift;


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
        ActivateSkill();
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
        canUseSkill = true;
        //Timer for cooldown
        if (skillCdTimer > 0) canUseSkill = false; 
        else
        {
            skillCdTimer = skillCd;
            HUDController.instance.AbilityCooldownHUD(controller.isWhite, skillIndex, skillCd);
        }
    }
    #endregion
}
