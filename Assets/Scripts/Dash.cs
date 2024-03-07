using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Dash : BaseSkill
{
    //Region dedicated to the different Variables.
    #region Variables

    public RaycastHit hit;
    public float dashDistance;

    [SerializeField] LayerMask holeLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] Portal portal;
    [SerializeField] Transform furthestGate;



    #endregion

    //Region deidcated to the different Getters/Setters.
    #region Getters/Setters

    #endregion

    //Region dedicated to methods native to Unity.
    #region Unity Functions

    private void Start()
    {
        hit.distance = dashDistance;
        portalLayer = LayerMask.GetMask("Portal");

        portal = GetComponent<Portal>();

        //Maybe not necesary, does fail in Portal.cs. this is also called in line 70
        portal.GetFurthestGate(this.transform);

    }
   


    #endregion

    //Region dedicated to Custom methods.
    #region Custom Methods
    public override void ActivateSkill()
    {
        if (Input.GetKeyDown(dashkey))
        {
            //Debug.Log("ActivateSkill");

            //Timer for cooldown
            if (skillCdTimer > 0) return;
            else skillCdTimer = skillCd;
            Debug.DrawRay(rcShootPoint.transform.position, rcShootPoint.transform.forward * dashDistance, Color.red);
            float tpDistance = dashDistance;

            //Shoots the raycast and do the dash
            if (Physics.Raycast(rcShootPoint.transform.position, rcShootPoint.transform.forward + transform.forward * dashDistance, out hit, dashDistance))
            {
                //Teleports the black wolf to the furthest gate in the portal
                //CHANGE TO BLACK WOLF IN FINAL VERSION
                if (controller.isWhite && hit.collider.gameObject.CompareTag("Portal") )
                {

                   //Debug.Log("Portal");
                    furthestGate = portal.GetFurthestGate(this.transform);
                    wolf.transform.position = furthestGate.position;
                }
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
            else
            {
                tpDistance = dashDistance;
            }

                Debug.Log(tpDistance);
                wolf.transform.position += wolf.transform.forward * tpDistance;
        }

    }
    #endregion
}
