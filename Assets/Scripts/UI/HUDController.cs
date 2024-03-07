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

    //Panel Specific to the HUD
    [SerializeField] private GameObject HUDpanel;

    //Panels to warn about taking/reciving Damage
    [SerializeField] private Image whiteHitPanel;
    [SerializeField] private Image blackHitPanel;

    private Coroutine whiteDamage;
    private Coroutine blackDamage;

    //The images that signify each ability
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
        abilityWhite = new();
        abilityBlack = new();

        foreach (GameObject icon in abilityWhiteIcon)
            abilityWhite.Add(new(icon, null));
        
        foreach (GameObject icon in abilityBlackIcon)
            abilityBlack.Add(new(icon, null));

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

    //Methods Relating to When the Players Use an Ability
    public void AddAbilityHUD(int ability)
    {
        abilityWhite[ability].Key.SetActive(true);
        abilityBlack[ability].Key.SetActive(true);
    }

    public void AbilityCooldownHUD(bool isWhite, int ability = 0, float cooldown = 2)
    {
        Coroutine abilityCoroutine = null;
        if (isWhite && abilityWhite[ability].Value == null)
        {
            abilityCoroutine = StartCoroutine(AbilityCooldown(ability, isWhite, cooldown));
            abilityWhite[ability] = new(abilityWhite[ability].Key, abilityCoroutine);
        }
        else if (abilityBlack[ability].Value == null)
        {
            abilityCoroutine = StartCoroutine(AbilityCooldown(ability, isWhite, cooldown));
            abilityBlack[ability] = new(abilityBlack[ability].Key, abilityCoroutine);
        }
    }

    //TODO: DELETE AFTER!!!!!!!!!!
    public void TestCooldown(bool isWhite)
    {
        AbilityCooldownHUD(isWhite);
    }
    private IEnumerator AbilityCooldown(int ability, bool isWhite, float cooldown) 
    {

        if (isWhite)
        {
            Image currentAbility = abilityWhite[ability].Key.transform.GetChild(0).GetComponent<Image>();

            currentAbility.fillAmount = 1;
            Tween cooldownAnimation = (currentAbility.DOFillAmount(0, cooldown));

            yield return cooldownAnimation.WaitForCompletion();

            abilityWhite[ability] = new(abilityWhite[ability].Key, null);
        }
        else
        {
            Image currentAbility = abilityBlack[ability].Key.transform.GetChild(0).GetComponent<Image>();

            currentAbility.fillAmount = 1;
            Tween cooldownAnimation = (currentAbility.DOFillAmount(0, cooldown));

            yield return cooldownAnimation.WaitForCompletion();

            abilityBlack[ability] = new(abilityBlack[ability].Key, null);
        }
    }

    // Methods Relating to When the Players take damage
    public void OnDamageHUD(bool isWhite)
    {

        Image currentImage = isWhite ? whiteHitPanel : blackHitPanel;

        if (isWhite && whiteDamage == null)
            whiteDamage = StartCoroutine(OnDamage(currentImage));

        else if (!isWhite && blackDamage == null)
            blackDamage = StartCoroutine(OnDamage(currentImage));
    }

    public void OnDamageStopHUD(bool isWhite)
    {
        Coroutine currentCoroutine = isWhite ? whiteDamage : blackDamage;
        Image currentImage = isWhite ? whiteHitPanel : blackHitPanel;
        StopCoroutine(currentCoroutine);
        currentImage.color = new Color(1, 1, 1, 0);

        if (isWhite)
            whiteDamage = null;
        else
            blackDamage = null;
    }
    private IEnumerator OnDamage(Image currentImage)
    {
        Debug.Log("start");
        do
        {
            currentImage.color = new Color(1, 1, 1, currentImage.color.a + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        } while (true);

    }

    #endregion
}
