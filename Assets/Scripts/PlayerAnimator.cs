using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Legs Features")]
    [SerializeField] private Vector3 legsConstraints;
    [SerializeField] private Vector3 legsSeparation;
    [SerializeField] private Vector3 legsOffset;
    [SerializeField] private float legsFlexion;

    [Header("Adaptation")]
    [SerializeField] private float anticipation;
    [SerializeField] private LayerMask groundLayer;
    private float speed;

    [Header("Walk Animation")]
    [SerializeField] private float stepHeigh;

    [Header("References")]
    [SerializeField] private Transform wolfModel;
    [SerializeField] private Transform armature;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;

    // Adaptation
    private Vector3 armatureOffset;
    private Vector3 leftHandOrigin;
    private Vector3 rightHandOrigin;
    private Vector3 leftFootOrigin;
    private Vector3 rightFootOrigin;
    private float leftHandDistance;
    private float rightHandDistance;
    private float leftFootDistance;
    private float rightFootDistance;

    // Setters
    private Vector2 moveInput = Vector2.zero;
    private Vector3 velocity = Vector2.zero;
    private bool isStillGrounded = false;
    public Vector2 MoveInput { set { moveInput = value; } }
    public Vector3 Velocity { set { velocity = value; } }
    public bool IsStillGrounded { set { isStillGrounded = value; } }

    // Walk Cycle
    private int side = -1;

    void Start()
    {
        StartCoroutine(WalkAnim());
    }

    private void Update()
    {
        if (isStillGrounded) speed = moveInput.magnitude / 4;
        else speed = Mathf.Abs(velocity.y);
    }

    // Comprueba la altura local a la que se debe apoyar cada pata y desliza los valores en función a la velocidad. 
    private void CheckGround()
    {
        Ray leftHandUpRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(leftHand.localPosition.x, legsConstraints.y, leftHand.localPosition.z )) + transform.forward * anticipation, wolfModel.TransformDirection(Vector3.down));
        RaycastHit leftHandUpHit;
        Ray rightHandUpRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(rightHand.localPosition.x, legsConstraints.y, rightHand.localPosition.z )) + transform.forward * anticipation, wolfModel.TransformDirection(Vector3.down));
        RaycastHit rightHandUpHit;
        Ray leftFootUpRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(leftFoot.localPosition.x, legsConstraints.y, leftFoot.localPosition.z )) + transform.forward * anticipation, wolfModel.TransformDirection(Vector3.down));
        RaycastHit leftFootUpHit;
        Ray rightFootUpRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(rightFoot.localPosition.x, legsConstraints.y, rightFoot.localPosition.z )) + transform.forward * anticipation, wolfModel.TransformDirection(Vector3.down));
        RaycastHit rightFootUpHit;

        Ray leftHandDownRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(leftHand.localPosition.x, 0.005f, leftHand.localPosition.z)), wolfModel.TransformDirection(Vector3.down));
        RaycastHit leftHandDownHit;
        Ray rightHandDownRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(rightHand.localPosition.x, 0.005f, rightHand.localPosition.z)), wolfModel.TransformDirection(Vector3.down));
        RaycastHit rightHandDownHit;
        Ray leftFootDownRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(leftFoot.localPosition.x, 0.005f, leftFoot.localPosition.z)), wolfModel.TransformDirection(Vector3.down));
        RaycastHit leftFootDownHit;
        Ray rightFootDownRay = new Ray(wolfModel.position + wolfModel.TransformDirection(new Vector3(rightFoot.localPosition.x, 0.005f, rightFoot.localPosition.z)), wolfModel.TransformDirection(Vector3.down));
        RaycastHit rightFootDownHit;

        // Left Hand
        if (Physics.Raycast(leftHandUpRay, out leftHandUpHit, legsConstraints.y, groundLayer) && leftHandUpHit.distance > 0.005f)
        {
            leftHandDistance = Mathf.MoveTowards(leftHandDistance, legsConstraints.y - leftHandUpHit.distance, Time.deltaTime * speed);
        }else if (Physics.Raycast(leftHandDownRay, out leftHandDownHit, legsConstraints.y, groundLayer))
        {
            leftHandDistance = Mathf.MoveTowards(leftHandDistance, - leftHandDownHit.distance, Time.deltaTime * speed);
        }
        else leftHandDistance = Mathf.MoveTowards(leftHandDistance, 0, Time.deltaTime * speed);
        // Right Hand
        if (Physics.Raycast(rightHandUpRay, out rightHandUpHit, legsConstraints.y, groundLayer) && rightHandUpHit.distance > 0.005f)
        {
            rightHandDistance = Mathf.MoveTowards(rightHandDistance, legsConstraints.y - rightHandUpHit.distance, Time.deltaTime * speed);
        }else if (Physics.Raycast(rightHandDownRay, out rightHandDownHit, legsConstraints.y, groundLayer))
        {
            rightHandDistance = Mathf.MoveTowards(rightHandDistance, -rightHandDownHit.distance, Time.deltaTime * speed);
        }
        else rightHandDistance = Mathf.MoveTowards(rightHandDistance, 0, Time.deltaTime * speed);
        // Left Foot
        if (Physics.Raycast(leftFootUpRay, out leftFootUpHit, legsConstraints.y, groundLayer) && leftFootUpHit.distance > 0.005f)
        {
            leftFootDistance = Mathf.MoveTowards(leftFootDistance, legsConstraints.y - leftFootUpHit.distance, Time.deltaTime * speed);
        }
        else if (Physics.Raycast(leftFootDownRay, out leftFootDownHit, legsConstraints.y, groundLayer))
        {
            leftFootDistance = Mathf.MoveTowards(leftFootDistance, -leftFootDownHit.distance, Time.deltaTime * speed);
        }
        else leftFootDistance = Mathf.MoveTowards(leftFootDistance, 0, Time.deltaTime * speed);
        // Right Foot
        if (Physics.Raycast(rightFootUpRay, out rightFootUpHit, legsConstraints.y, groundLayer) && rightFootUpHit.distance > 0.005f)
        {
            rightFootDistance = Mathf.MoveTowards(rightFootDistance, legsConstraints.y - rightFootUpHit.distance, Time.deltaTime * speed);
        }
        else if (Physics.Raycast(rightFootDownRay, out rightFootDownHit, legsConstraints.y, groundLayer))
        {
            rightFootDistance = Mathf.MoveTowards(rightFootDistance, -rightFootDownHit.distance, Time.deltaTime * speed);
        }
        else rightFootDistance = Mathf.MoveTowards(rightFootDistance, 0, Time.deltaTime * speed);
        
    }

    // Establece el centro de la animación de cada pata.
    private void SetOrigins()
    {
        float lessDistance = Mathf.Min(leftHandDistance, rightHandDistance, leftFootDistance, rightFootDistance);
        float greatestDistance = Mathf.Max(leftHandDistance, rightHandDistance, leftFootDistance, rightFootDistance);

        //armatureOffset = Vector3.MoveTowards(armature.localPosition, new Vector3(0, (lessDistance + greatestDistance) / 2 - legsFlexion, 0), Time.deltaTime * planeVelocity / 2 + Time.deltaTime * Mathf.Abs(verticalVelocity));
        armatureOffset = Vector3.MoveTowards(armature.localPosition, new Vector3(0, (lessDistance + greatestDistance) / 2 - legsFlexion, 0), speed * Time.fixedDeltaTime);
        
        armature.localPosition = armatureOffset;

        leftHandOrigin = legsOffset + new Vector3(-legsSeparation.x / 2, legsSeparation.y / 2, legsSeparation.z / 2) + Vector3.up * (leftHandDistance) - armatureOffset;
        rightHandOrigin = legsOffset + new Vector3(legsSeparation.x / 2, legsSeparation.y / 2, legsSeparation.z / 2) + Vector3.up * (rightHandDistance) - armatureOffset;
        leftFootOrigin = legsOffset + new Vector3(-legsSeparation.x / 2, -legsSeparation.y / 2, -legsSeparation.z / 2) + Vector3.up * (leftFootDistance) - armatureOffset;
        rightFootOrigin = legsOffset + new Vector3(legsSeparation.x / 2, -legsSeparation.y / 2, -legsSeparation.z / 2) + Vector3.up * (rightFootDistance) - armatureOffset;
    }

    // Añade a las patas la animación de andar.
    private IEnumerator WalkAnim()
    {
        do
        {
            Vector3 lOffset = Vector3.forward * legsConstraints.z * side;

            for (float i = 0; i < 1; i += Time.deltaTime * new Vector3(velocity.x, 0, velocity.z).magnitude)
            {
                CheckGround();
                SetOrigins();

                float t = Mathf.Sin(i * 3.14f - 3.14f / 2) / 2 + 0.5f;
                
                leftHand.localPosition = Vector3.Lerp(leftHandOrigin - lOffset, leftHandOrigin + lOffset, t) + new Vector3(0,stepHeigh/2 + stepHeigh/2 * side,0) * Mathf.Sin(i * 3.14f);
                rightHand.localPosition = Vector3.Lerp(rightHandOrigin + lOffset, rightHandOrigin - lOffset, t) + new Vector3(0, stepHeigh / 2 + stepHeigh / 2 * -side, 0) * Mathf.Sin(i * 3.14f);
                leftFoot.localPosition = Vector3.Lerp(leftFootOrigin + lOffset, leftFootOrigin - lOffset, t) + new Vector3(0, stepHeigh / 2 + stepHeigh / 2 * -side, 0) * Mathf.Sin(i * 3.14f);
                rightFoot.localPosition = Vector3.Lerp(rightFootOrigin - lOffset, rightFootOrigin + lOffset, t) + new Vector3(0, stepHeigh / 2 + stepHeigh / 2 * side, 0) * Mathf.Sin(i * 3.14f);

                yield return null;
            }
            side = -side;

        }while (true);
    }
}
