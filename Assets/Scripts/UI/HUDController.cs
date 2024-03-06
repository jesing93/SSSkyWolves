using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HUDController : MonoBehaviour
{
    //Region dedicated to the different Variables.
    #region Variables
    public static HUDController instance;

    public GameObject HUDpanel;

    [SerializeField] private List<GameObject> abilityWhiteIcon;
    [SerializeField] private List<GameObject> abilityBlackIcon;


     private List<KeyValuePair<GameObject, Coroutine>> abilityWhite;
     private List<KeyValuePair<GameObject, Coroutine>> abilityBlack;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        HUDpanel.SetActive(false);

        foreach (GameObject icon in abilityWhiteIcon)
            abilityWhite[abilityWhiteIcon.IndexOf(icon)] = new(abilityWhite[abilityWhiteIcon.IndexOf(icon)].Key, null);
        
        foreach (GameObject icon in abilityBlackIcon)
            abilityBlack[abilityBlackIcon.IndexOf(icon)] = new(abilityBlack[abilityWhiteIcon.IndexOf(icon)].Key, null);


        foreach (KeyValuePair<GameObject, Coroutine> item in abilityWhite)
            item.Key.SetActive(false);

        foreach (KeyValuePair<GameObject, Coroutine> item in abilityBlack)
            item.Key.SetActive(false);
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods

    public void ActivateHUD() => HUDpanel.SetActive(true);
    public void DeactivateHUD() => HUDpanel.SetActive(false);

    public void AddAbilityHUD(int ability)
    {
        abilityWhite[ability].Key.SetActive(true);
        abilityBlack[ability].Key.SetActive(true);
    }

    public void AbilityCooldownHUD(int ability, bool isWhite, float cooldown)
    {
        if (isWhite && abilityWhite[ability].Value != null)
        {
            abilityWhite[ability] = new(abilityWhite[ability].Key, StartCoroutine(AbilityCooldown(ability, isWhite, cooldown)));
        }
        else if (abilityBlack[ability].Value != null)
        {
            abilityBlack[ability] = new(abilityBlack[ability].Key, StartCoroutine(AbilityCooldown(ability, isWhite, cooldown)));
        }
    }

    private IEnumerator AbilityCooldown(int ability, bool isWhite, float cooldown) 
    {
     
        
        yield return new WaitForSeconds(0.0f);

        if (isWhite)
        {
            Image currentAbility = abilityWhite[ability].Key.transform.GetChild(0).GetComponent<Image>();
            
            Tween cooldownAnimation = (currentAbility.DOFillAmount(0, cooldown));

            yield return cooldownAnimation.WaitForCompletion();

            abilityWhite[ability] = new(abilityWhite[ability].Key, null);
        }
        else
        {
            Image currentAbility = abilityBlack[ability].Key.transform.GetChild(0).GetComponent<Image>();

            Tween cooldownAnimation = (currentAbility.DOFillAmount(0, cooldown));

            yield return cooldownAnimation.WaitForCompletion();

            abilityBlack[ability] = new(abilityBlack[ability].Key, null);
        }
    }
    #endregion
}
