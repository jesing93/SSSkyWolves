using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] private float movementTensor;
    [SerializeField] private float movementDeath;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool calculateWithSine;
    private Vector2 moveInputTarget;
    private Vector2 moveInput;

    [Header("Adaptation")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float step;
    [SerializeField] private float adaptationSpeed;

    [Header("Data")]
    [SerializeField] private PlayerCameraData cameraData;

    [Header("Interaction")]
    [SerializeField]private List<Transform> interactions;

    [SerializeField] private Transform playerSnap;

    private BaseInteraction currentInteraction;
    private bool isBusy;
    private bool inLockedInteraction;
    private bool restrictedInteraction;

    [Header("Other")]
    public bool isWhite;
    private bool isInLight;
    float lightTime;
    float maxTimeToDie = 1;

    // References
    [Header("References")]
    public Transform frontDetection;
    public Transform backDetection;
    private Rigidbody rb;
    private PlayerAnimator panim;
    private Animator anim;
    private GameObject rel;
    private PlayerCamera cam;
    #endregion

    #region Getters / Setters
    public bool IsInLight { get => isInLight; set => isInLight = value; }
    public Transform PlayerSnap { get => playerSnap; set => playerSnap = value; }
    public bool InLockedInteraction { get => inLockedInteraction; set => inLockedInteraction = value; }
    public bool RestrictedInteraction { get => restrictedInteraction; set => restrictedInteraction = value; }
    public bool IsBusy { get => isBusy; set => isBusy = value; }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        interactions = new();
    }
    void Start()
    {
        GetReferences();
    }
    private void Update()
    {
        InputsModifiers();
        AdaptToTheTerrain();
    }
    void FixedUpdate()
    {
        Move();

        //OvercomeStep();
        //LightTimeCheck();
    }
    void LateUpdate()
    {
        panim.speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
    }
    #endregion

    #region Custom Methods
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
        panim = GetComponent<PlayerAnimator>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rel = new GameObject();
        cam = new GameObject().AddComponent<PlayerCamera>();
        cam.SetUp(this.transform, cameraData);
    }

    //***** Inputs Zone *****//
    public void OnMove(Vector2 moveInput)
    {
        this.moveInputTarget = moveInput;
    }

    //TODO:almacenar Interaciones
    public void AddInteraction(Transform interaction)
    {
        BaseInteraction interactionType = interaction.GetComponent<BaseInteraction>();
        interactions.Add(interaction);
    }
    public void RemoveInteraction(Transform interaction)
    {
        interactions.Remove(interaction);
    }


    //Might change later on
    public void OnInteract()
    {

        if (interactions.Count > 0 && !IsBusy)
        {
            Transform closestInteraction = interactions[0].transform;

            //Checks for the closest Interactable in range
            foreach (var interaction in interactions)
                if (Vector3.Distance(interaction.position, transform.position) >= Vector3.Distance(closestInteraction.position, transform.position))
                    closestInteraction = interaction.transform;

            currentInteraction = closestInteraction.GetComponent<BaseInteraction>();
            StartCoroutine(closestInteraction.GetComponent<BaseInteraction>().InteractionEnter(this));
            
            IsBusy = true;
            rb.velocity = Vector3.zero;
        }

        else if (currentInteraction?.GetType() == typeof(ContinuousInteraction))
        {
            
            StartCoroutine(currentInteraction.InteractionExit());
            currentInteraction = null;
            IsBusy = false;
        }
    }

    public void EndInteraction()
    {
        currentInteraction = null;
        IsBusy = false;
    }




    private void InputsModifiers()
    {
        if (moveInputTarget == Vector2.zero)
            moveInput = Vector2.MoveTowards(moveInput, Vector2.zero, Time.deltaTime * movementDeath);
        else
            moveInput = Vector2.MoveTowards(moveInput, moveInputTarget, Time.deltaTime * movementTensor);
    }

    //***** Movement *****//
    private void Move()
    {
        if (!inLockedInteraction)
        {
            Vector3 direction;
            if (restrictedInteraction)
            {
                Transform snapPoint = currentInteraction.ConvertTo<ContinuousInteraction>().CurrentSnapPoint.transform;
                direction = new Vector3(0, 0, moveInput.y);
                direction = Quaternion.LookRotation(snapPoint.forward, snapPoint.up) * direction;
            }
            else
            {
                direction = new((Mathf.Sin(moveInput.x * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(moveInput.x), 0, (Mathf.Sin(moveInput.y * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(moveInput.y));
            }

            if (direction.magnitude > 1) direction.Normalize();
            direction *= movementSpeed;

            rel.transform.position = transform.position;
            rel.transform.LookAt(transform.position + direction);

            direction.y = rb.velocity.y;

            rb.velocity = direction;
            if (!restrictedInteraction)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, rel.transform.eulerAngles.y, transform.eulerAngles.z)), rotationSpeed * Time.deltaTime);
        }
    }

    private void AdaptToTheTerrain()
    {
        Ray forwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0, step, 0.65f)), Vector3.down);
        RaycastHit forwardHit;
        Ray backwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0, step, -0.65f)), Vector3.down);
        RaycastHit backwardHit;

        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0, step, 0.65f)), Vector3.down, Color.red);
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0, step, -0.65f)), Vector3.down, Color.red);

        Vector3 fPoint = forwardSensor.GetPoint(step);
        Vector3 bPoint = forwardSensor.GetPoint(step);

        if (Physics.Raycast(forwardSensor, out forwardHit, step * 2, groundLayer) && forwardHit.distance > 0.01f)
        {
            fPoint = forwardHit.point;
        }
        if (Physics.Raycast(backwardSensor, out backwardHit, step * 2, groundLayer) && backwardHit.distance > 0.01f)
        {
            bPoint = backwardHit.point;
        }

        rel.transform.position = bPoint;
        rel.transform.LookAt(fPoint);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(rel.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)), adaptationSpeed * Time.deltaTime);
    }
    #endregion

    //***** Others *****//
    private void LightTimeCheck()
    {
        if ((isWhite && isInLight) || (!isWhite && !isInLight))
        {
            lightTime = 0;
        }
        else
        {
            lightTime += Time.deltaTime;
        }
        if (lightTime >= maxTimeToDie)
        {
            //TODO: Die
            //Debug.Log("Die");
            //GameManager.Instance.WolfDeath();
        }
    }
}
