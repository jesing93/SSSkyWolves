using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Dash : BaseSkill
{
    //Region dedicated to the different Variables.
    #region Variables

    public float dashDistance;

    [SerializeField] Portal portal;
    [SerializeField] Transform furthestGate;

    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public override void ActivateSkill()
    {
        if (Input.GetKeyDown(dashkey))
        {
            base.ActivateSkill();
            Debug.DrawRay(rcShootPoint.transform.position, rcShootPoint.transform.forward * dashDistance, Color.red);
            float tpDistance = dashDistance;
            RaycastHit hit;
            bool isPortal = false;

            //Shoots the raycast and do the dash
            if (Physics.Raycast(rcShootPoint.transform.position, rcShootPoint.transform.forward + transform.forward * dashDistance, out hit, dashDistance))
            {
                //Teleports the black wolf to the furthest gate in the portal
                //TODO: CHANGE TO BLACK WOLF IN FINAL VERSION
                if (controller.isWhite && hit.collider.gameObject.CompareTag("Portal") )
                {
                    portal = hit.collider.gameObject.GetComponent<Portal>();
                    furthestGate = portal.GetFurthestGate(transform);
                    wolf.transform.position = furthestGate.position;
                    isPortal = true;
                }
                else
                {
                    //Sets the distance of the dash to max distance possible if it hits an object
                    if (hit.distance < dashDistance)
                    {
                        tpDistance = Mathf.Min(dashDistance, Mathf.Max(0, hit.distance));
                    }
                    else
                    {
                        tpDistance = dashDistance;
                    }
                }
            }
            else
            {
                tpDistance = dashDistance;
            }

            if(!isPortal)
                wolf.transform.position += wolf.transform.forward * tpDistance;
        }

    }
    #endregion
}
