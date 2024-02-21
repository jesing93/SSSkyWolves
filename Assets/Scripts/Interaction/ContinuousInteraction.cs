using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousInteraction : BaseInteraction
{
    //Region dedicated to the different Variables.
    #region Variables
    private bool isBusy;
    [SerializeField]private Transform currentSnapPoint;
    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions
    protected void Update()
    {
        if (isBusy)
        {
            InteractionStay();
        }
    }
    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    //Method to start the interaction
    public override IEnumerator InteractionEnter(PlayerController player)
    {
        yield return StartCoroutine(base.InteractionEnter(player));
        isBusy = true;

        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                player.InLockedInteraction = true;
                currentPlayer.RestrictedInteraction = true;
                if (hasOrientation)
                {
                    ClosestSnapPoint();
                    currentPlayer.transform.rotation = currentSnapPoint.transform.rotation;

                    currentPlayer.transform.position = new Vector3(currentSnapPoint.transform.position.x,player.transform.position.y, currentSnapPoint.transform.position.z);
                }

                //TODO: Make the object the parent of the wolf, limiting its movement
                break;
            case (InteractionType.GrabSmall):
                //TODO: Make the wolf the parent of the wolf, letting it move freely
                transform.position = player.PlayerSnap.position;
                transform.parent = player.transform;

                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }
                break;
            default: break;
        }
    }
    //Method repeated whilst the interaction takes place
    public void InteractionStay()
    {
        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                //TODO: Detects wether to move or not 
                break;
            case InteractionType.GrabSmall:
                //TODO: Not sure
                break;
            default: break;
        }
    }
    //Method to end the interaction
    public override IEnumerator InteractionExit()
    {

        switch (interactionType)
        {
            case InteractionType.GrabLarge:
                currentPlayer.InLockedInteraction = false;
                currentPlayer.RestrictedInteraction = false;
                //TODO: Unparents the objects 
                break;
            case InteractionType.GrabSmall:
                transform.parent = null;

                foreach (Collider collider in colliders)
                {
                    collider.enabled = true;
                }
                break;
            default: break;
        }
        base.InteractionExit();
        isBusy = false;
        yield return StartCoroutine(base.InteractionExit());
    }

    public void ClosestSnapPoint()
    {

        Transform closestInteraction = snapPoints[0].transform;
        foreach (var snapPoint in snapPoints)
                if (Vector3.Distance(closestInteraction.position, currentPlayer.transform.position) >= Vector3.Distance(snapPoint.position, currentPlayer.transform.position))
                    closestInteraction = snapPoint.transform;
        currentSnapPoint = closestInteraction;
    }
    #endregion
}
