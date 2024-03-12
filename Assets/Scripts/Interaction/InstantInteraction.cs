using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class InstantInteraction : BaseInteraction
{
    //Region dedicated to the different Variables.
    #region Variables
    [SerializeField]private List<LightSource> lightSources;
    [SerializeField] private float lightTimeStart;
    [SerializeField] private float lightTimeOut;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    private void Start()
    {
        lightSources = new();

        foreach (LightSource light in GetComponentsInChildren<LightSource>())
            lightSources.Add(light);
        
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    //Method to start the interaction
    public override IEnumerator InteractionEnter(PlayerController player)
    {
        Debug.Log("Entered");
        base.InteractionEnter(player);
        switch(interactionType)
        {
            case InteractionType.SwitchLights:
                yield return new WaitForSeconds(lightTimeStart);

                foreach (LightSource light in lightSources)
                    light.Switch();

                yield return new WaitForSeconds(lightTimeOut);
                
                break;
            default: break;
        }
        yield return StartCoroutine(base.InteractionEnter(player));

        StartCoroutine(InteractionExit());
    }
    //Method to end the interaction
    public override IEnumerator InteractionExit()
    {
        switch (interactionType)
        {
            case InteractionType.SwitchLights:
                foreach (LightSource light in lightSources)
                    light.Switch();
                break;
            default: break;
        }
        base.InteractionExit();
        yield return StartCoroutine(base.InteractionExit());
    }
    #endregion
}
