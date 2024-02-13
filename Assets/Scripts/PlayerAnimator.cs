using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Paremeters")]
    [SerializeField] private Vector3 legsConstraints;
    [SerializeField] private Vector3 legsOffset;
    [SerializeField] private Vector3 legsSeparation;
    [SerializeField] private LayerMask groundLayer;

    [Header("Walk")]
    [SerializeField] private float stepHeigh;

    [Header("References")]
    [SerializeField] private Transform armature;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;

    private Vector3 leftHandOrigin;
    private RaycastHit leftHandHit;
    private Vector3 rightHandOrigin;
    private RaycastHit rightHandHit;
    private Vector3 leftFootOrigin;
    private RaycastHit leftFootHit;
    private Vector3 rightFootOrigin;
    private RaycastHit rightFootHit;

    public float speed = 0;
    private float armatureOffset;
    private int side = -1;

    void Start()
    {
        StartCoroutine(WalkAnim());
    }

    void Update()
    {
        CheckGround();
        SetOrigins();
    }
    
    private void CheckGround()
    {
        Ray leftHandRay = new Ray(transform.position + transform.TransformDirection(new Vector3(leftHand.localPosition.x, 1, leftHand.localPosition.z)), transform.TransformDirection(Vector3.down));
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(leftHand.localPosition.x, 1, leftHand.localPosition.z)), transform.TransformDirection(Vector3.down), Color.red, Time.deltaTime);
        Ray rightHandRay = new Ray(transform.position + transform.TransformDirection(new Vector3(rightHand.localPosition.x, 1, rightHand.localPosition.z)), transform.TransformDirection(Vector3.down));
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(rightHand.localPosition.x, 1, rightHand.localPosition.z)), transform.TransformDirection(Vector3.down), Color.red, Time.deltaTime);
        Ray leftFootRay = new Ray(transform.position + transform.TransformDirection(new Vector3(leftFoot.localPosition.x, 1, leftFoot.localPosition.z)), transform.TransformDirection(Vector3.down));
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(leftFoot.localPosition.x, 1, leftFoot.localPosition.z)), transform.TransformDirection(Vector3.down), Color.red, Time.deltaTime);
        Ray rightFootRay = new Ray(transform.position + transform.TransformDirection(new Vector3(rightFoot.localPosition.x, 1, rightFoot.localPosition.z)), transform.TransformDirection(Vector3.down));
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(rightFoot.localPosition.x, 1, rightFoot.localPosition.z)), transform.TransformDirection(Vector3.down), Color.red, Time.deltaTime);

        if (Physics.Raycast(leftHandRay, out leftHandHit, 100, groundLayer))
        {

        }
        if (Physics.Raycast(rightHandRay, out rightHandHit, 100, groundLayer))
        {

        }
        if (Physics.Raycast(leftFootRay, out leftFootHit, 100, groundLayer))
        {

        }
        if (Physics.Raycast(rightFootRay, out rightFootHit, 100, groundLayer))
        {

        }
    }
    private void SetOrigins()
    {
        leftHandOrigin = legsOffset + new Vector3(-legsSeparation.x / 2, legsSeparation.y / 2, legsSeparation.z / 2) + Vector3.up * (-leftHandHit.distance + 1);
        rightHandOrigin = legsOffset + new Vector3(legsSeparation.x / 2, legsSeparation.y / 2, legsSeparation.z / 2) + Vector3.up * (-rightHandHit.distance + 1);
        leftFootOrigin = legsOffset + new Vector3(-legsSeparation.x / 2, -legsSeparation.y / 2, -legsSeparation.z / 2) + Vector3.up * (-leftFootHit.distance + 1);
        rightFootOrigin = legsOffset + new Vector3(legsSeparation.x / 2, -legsSeparation.y / 2, -legsSeparation.z / 2) + Vector3.up * (-rightFootHit.distance + 1);
    }
    private IEnumerator WalkAnim()
    {
        do
        {
            Vector3 lOffset = Vector3.forward * legsConstraints.z * side;

            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                float t = Mathf.Sin(i * 3.14f - 3.14f / 2) / 2 + 0.5f;
                float greatestDistance = Mathf.Max(leftHandHit.distance, rightHandHit.distance, leftFootHit.distance, rightFootHit.distance);
                Vector3 armatureOffset = Vector3.zero;
                if (greatestDistance > 1.1f)
                    armatureOffset = new Vector3(0, -greatestDistance + 1.1f, 0);
                else
                {
                    armatureOffset = new Vector3(0, 0, 0);
                }
                
                armature.localPosition = armatureOffset;
                leftHand.localPosition = Vector3.Lerp(leftHandOrigin - lOffset, leftHandOrigin + lOffset, t) + new Vector3(0,stepHeigh/2 + stepHeigh/2 * side,0) * Mathf.Sin(i * 3.14f) - armatureOffset;
                rightHand.localPosition = Vector3.Lerp(rightHandOrigin + lOffset, rightHandOrigin - lOffset, t) + new Vector3(0, stepHeigh / 2 + stepHeigh / 2 * -side, 0) * Mathf.Sin(i * 3.14f) - armatureOffset;
                leftFoot.localPosition = Vector3.Lerp(leftFootOrigin + lOffset, leftFootOrigin - lOffset, t) + new Vector3(0, stepHeigh / 2 + stepHeigh / 2 * -side, 0) * Mathf.Sin(i * 3.14f) - armatureOffset;
                rightFoot.localPosition = Vector3.Lerp(rightFootOrigin - lOffset, rightFootOrigin + lOffset, t) + new Vector3(0, stepHeigh / 2 + stepHeigh / 2 * side, 0) * Mathf.Sin(i * 3.14f) - armatureOffset;

                yield return null;
            }
            side = -side;

        }while (true);

        
    }
}
