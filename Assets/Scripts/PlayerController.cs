using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    [SerializeField] private float gravity;
    private Vector2 moveInputTarget;
    private Vector2 moveInput;
    private Vector2 yAxisCap = new (-100, 100);
    private float yAxis;

    [Header("Ground")]
    [SerializeField] private Vector3 groundSensorsOffset;
    [SerializeField] private LayerMask groundLayer;
    private bool isStillGrounded;
    private bool isDifficultTerrain;
    private Vector3 groundNormal;

    [Header("Adaptation")]
    [SerializeField] private Vector3 adaptationSensorsOffset;
    [SerializeField] private float maxRampInclination;
    [SerializeField] private float rampSlideTensor;
    [SerializeField] private float rampSlideDeath;
    [SerializeField] private float maxRampSlideForce;
    [SerializeField] private float maxStepHeight;
    [SerializeField] private float adaptationSpeed;
    [SerializeField] private float adaptationAnticipation;
    [SerializeField] private Transform wolfModel;
    private float rampSlideForce;

    [Header("Data")]
    //[SerializeField] private PlayerCameraData cameraData;

    [Header("Interaction")]
    [SerializeField]private List<Transform> interactions;
    [SerializeField] private Transform playerSnap;
    private BaseInteraction currentInteraction;
    private bool isBusy;
    private bool inLockedInteraction;

    [Header("Other")]
    public bool isWhite;
    private bool isProtected = false;

    [Header("References")]
    public Transform frontDetection;
    public Transform backDetection;
    public ParticleController lightDamagePaticles;
    private Rigidbody rb;
    private PlayerAnimator panim;
    private Animator anim;
    private GameObject rel;

    CapsuleCollider[] cols;
    Vector3[] colsOffsets;

    private bool isInLight;
    float lightTime;
    float maxTimeToDie = 1;

    #endregion

    #region Getters / Setters
    public bool IsInLight { get => isInLight; }
    public Transform PlayerSnap { get => playerSnap; set => playerSnap = value; }
    public bool InLockedInteraction { get => inLockedInteraction; set => inLockedInteraction = value; }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        interactions = new();
    }
    void Start()
    {
        GetReferences();
        colsOffsets = new Vector3[cols.Length];
        for (int i = 0; i < cols.Length; i++) { colsOffsets[i] = cols[i].center; }
    }
    private void Update()
    {
        InputsModifiers();
        
    }
    void FixedUpdate()
    {
        
        Move();
        OvercomeStep();
        LightTimeCheck();

    }
    void LateUpdate()
    {
        CheckGround();
        Adaptation();
        
        panim.Velocity = rb.velocity;
        panim.MoveInput = moveInput * movementSpeed;
        panim.IsStillGrounded = isStillGrounded;
    }
    #endregion

    #region Custom Methods
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
        panim = GetComponent<PlayerAnimator>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rel = new GameObject();
        cols = GetComponents<CapsuleCollider>();
    }

    //***** Inputs Zone *****//
    public void OnMove(Vector2 moveInputTarget)
    {
        this.moveInputTarget = moveInputTarget;
    }

    //TODO:almacenar Interaciones
    public void AddInteraction(Transform interaction)
    {
        interactions.Add(interaction);
    }
    public void RemoveInteraction(Transform interaction)
    {
        interactions.Remove(interaction);
    }


    //Might change later on
    public void OnInteract()
    {
        if (interactions.Count > 0 && !isBusy)
        {
            Transform closestInteraction = interactions[0].transform;

            //Checks for the closest Interactable in range
            foreach (var interaction in interactions)
                if (Vector3.Distance(interaction.position, transform.position) >= Vector3.Distance(closestInteraction.position, transform.position))
                    closestInteraction = interaction.transform;

            currentInteraction = closestInteraction.GetComponent<BaseInteraction>();
            StartCoroutine(closestInteraction.GetComponent<BaseInteraction>().InteractionEnter(this));
            
            isBusy = true;
        }

        else if (currentInteraction?.GetType() == typeof(ContinuousInteraction))
        {
            
            StartCoroutine(currentInteraction.InteractionExit());
            currentInteraction = null;
            isBusy = false;
        }
    }

    public void EndInteraction()
    {
        currentInteraction = null;
        isBusy = false;
    }

    private void InputsModifiers()
    {
        if (moveInputTarget == Vector2.zero)
            moveInput = Vector2.MoveTowards(moveInput, Vector2.zero, Time.deltaTime * movementDeath);
        else
            moveInput = Vector2.MoveTowards(moveInput, moveInputTarget, Time.deltaTime * movementTensor);
    }

    //***** Movement *****//

    // Comprueba el suelo en las direcciones establecidas y cambia el cap del ejeY y determina la angulacion del terreno mas alto.
    private void CheckGround()
    {
        Vector3Int[] directions = new Vector3Int[] { new (-1, 1, 1), new(1, 1, 1), new(-1, 1, -1), new(1, 1, -1), new(-1, 1, 0), new(1, 1, 0) };
        float lessDistance = 5000;
        groundNormal = Vector3.up;

        for(int i = 0; i < directions.Length; i++)
        {
            Ray groundRay = new Ray(transform.position + transform.TransformDirection(new Vector3(groundSensorsOffset.x * directions[i].x, groundSensorsOffset.y * directions[i].y, groundSensorsOffset.z * directions[i].z)), Vector3.down);
            RaycastHit groundRayHit;
            if (Physics.Raycast(groundRay, out groundRayHit, maxStepHeight + groundSensorsOffset.y, groundLayer))
            {
                if(groundRayHit.distance < lessDistance){
                    lessDistance = groundRayHit.distance;
                    groundNormal = groundRayHit.normal;
                }
            }
        }

        var cross = Vector3.Cross(transform.right, groundNormal);
        var _lookRotation = Quaternion.LookRotation(cross, groundNormal);
        float xRot = _lookRotation.eulerAngles.x;
        if (xRot > 180) xRot = 360 - xRot;

        if (lessDistance < maxStepHeight + groundSensorsOffset.y)
        {
            if (xRot > maxRampInclination)
            {
                isDifficultTerrain = true;
                rampSlideForce += Time.deltaTime * rampSlideTensor;
            }
            else
            {
                if (lessDistance < groundSensorsOffset.y + 0.05f)
                {
                    isStillGrounded = true;
                    yAxisCap.x = 0;
                }
                rampSlideForce -= Time.deltaTime * rampSlideDeath;
            }
        }
        else
        {
            isStillGrounded = false;
            rampSlideForce -= Time.deltaTime * rampSlideDeath;
            yAxisCap.x = -25;
        }
    }

    // Setea la velocidad del rigidbody en funcion de los ejes de entrada y la gravedad calculada. Ademas rota al personaje hacia la direccion a la que se mueve.
    private void Move()
    {
        Vector3 direction;
        direction = new((Mathf.Sin(moveInput.x * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(moveInput.x), 0, (Mathf.Sin(moveInput.y * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(moveInput.y));

        if (direction.magnitude > 1) direction.Normalize();
        direction *= movementSpeed;
        rel.transform.position = transform.position;
        rel.transform.LookAt(transform.position + direction);

        yAxis = Mathf.Clamp(yAxis, yAxisCap.x, yAxisCap.y);
        direction.y = yAxis;

        Vector3 cross = Vector3.Cross(transform.right, groundNormal);
        rampSlideForce = Mathf.Clamp(rampSlideForce, 0, maxRampSlideForce);
        Vector3 rampSlide = -cross * movementSpeed * rampSlideForce;
        direction += rampSlide;

        rb.velocity = direction;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, rel.transform.eulerAngles.y, transform.eulerAngles.z)), rotationSpeed * moveInput.magnitude * Time.fixedDeltaTime);

        yAxis += gravity * Time.fixedDeltaTime;
    }

    // Calcula la inclinacion media del terreno y rota el modelo 3D hasta alcanzarla.
    private void Adaptation()
    {
        rel.transform.position = Vector3.zero;
        rel.transform.eulerAngles = new Vector3(-maxRampInclination, 0, 0);
        float adaptationSensorHeight = rel.transform.TransformDirection(0, 0, groundSensorsOffset.z + adaptationAnticipation).y;
        if(adaptationSensorHeight < maxStepHeight) adaptationSensorHeight = maxStepHeight;

        Ray forwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0, 0, groundSensorsOffset.z + adaptationAnticipation)) + Vector3.up * adaptationSensorHeight, Vector3.down);
        RaycastHit forwardHit;
        Ray backwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0, 0, -groundSensorsOffset.z + adaptationAnticipation / 2)) + Vector3.up * adaptationSensorHeight, Vector3.down);
        RaycastHit backwardHit;
        Ray forwardShortSensor = new(transform.position + transform.TransformDirection(new Vector3(0, 0, groundSensorsOffset.z)), Vector3.down);
        RaycastHit forwardShortHit;
        Ray backwardShortSensor = new(transform.position + transform.TransformDirection(new Vector3(0, 0, -groundSensorsOffset.z)), Vector3.down);
        RaycastHit backwardShortHit;

        Vector3 fPoint = forwardSensor.GetPoint(adaptationSensorHeight);
        Vector3 bPoint = backwardSensor.GetPoint(adaptationSensorHeight);

        if (Physics.Raycast(forwardSensor, out forwardHit, adaptationSensorHeight, groundLayer) && forwardHit.distance > 0.005f)
        {
            fPoint = forwardHit.point;
        }
        else if (Physics.Raycast(forwardShortSensor, out forwardShortHit, adaptationSensorHeight, groundLayer) && forwardShortHit.distance > 0.005f)
        {
            fPoint = forwardShortHit.point;
        }

        if (Physics.Raycast(backwardSensor, out backwardHit, adaptationSensorHeight, groundLayer) && backwardHit.distance > 0.005f)
        {
            bPoint = backwardHit.point;
        }
        else if (Physics.Raycast(backwardShortSensor, out backwardShortHit, adaptationSensorHeight, groundLayer) && backwardShortHit.distance > 0.005f)
        {
            bPoint = backwardShortHit.point;
        }

        rel.transform.position = bPoint;
        rel.transform.LookAt(fPoint);

        float rotationSpeed = adaptationSpeed * moveInput.magnitude * movementSpeed / 4;
        if (!isStillGrounded) rotationSpeed = Mathf.Abs(rb.velocity.y);

        wolfModel.transform.rotation = Quaternion.RotateTowards(wolfModel.transform.rotation, Quaternion.Euler(new Vector3(rel.transform.eulerAngles.x, wolfModel.transform.eulerAngles.y, wolfModel.transform.eulerAngles.z)), rotationSpeed * Time.deltaTime);
    }

    // Busca escalones o diferencias de altura que pueda sortear, establece la posicion del collider en la mas alta y mueve al personaje hacia ella.
    private void OvercomeStep()
    {
        if (!isStillGrounded || isDifficultTerrain)
        {
            for(int i = 0; i < cols.Length; i++) { cols[i].center = colsOffsets[i]; }
            wolfModel.localPosition = Vector3.MoveTowards(wolfModel.localPosition, Vector3.zero, Mathf.Abs(rb.velocity.y) * Time.fixedDeltaTime);
            return;
        }

        Vector3Int[] directions = new Vector3Int[] { new(-1, 1, 1), new(1, 1, 1), new(-1, 1, -1), new(1, 1, -1), new(-1, 1, 0), new(1, 1, 0) };
        
        Vector3 normal = Vector3.zero;
        float greaterHeight = -1000;

        for (int i = 0; i < directions.Length; i++)
        {
            Ray stepSensor = new Ray(transform.position + transform.TransformDirection(new Vector3(adaptationSensorsOffset.x * directions[i].x, maxStepHeight * directions[i].y, adaptationSensorsOffset.z * directions[i].z)), Vector3.down);
            RaycastHit sensorHit;
            if (Physics.Raycast(stepSensor, out sensorHit, maxStepHeight * 2, groundLayer) && sensorHit.distance > 0.005f)
            {
                if (sensorHit.point.y > greaterHeight)
                {
                    greaterHeight = sensorHit.point.y;
                    normal = sensorHit.normal;
                }
            }
        }
        var cross = Vector3.Cross(transform.right, normal);
        var _lookRotation = Quaternion.LookRotation(cross, normal);
        float xRot = _lookRotation.eulerAngles.x;
        if (xRot > 180) xRot = 360 - xRot;
        if (greaterHeight == -1000 || xRot > maxRampInclination) greaterHeight = transform.position.y;

        float yOffset = greaterHeight - transform.position.y;
        Vector3 lastPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up * yOffset, moveInput.magnitude * movementSpeed / 4 * Time.fixedDeltaTime);
        for (int i = 0; i < cols.Length; i++) { cols[i].center = colsOffsets[i] + Vector3.up * yOffset; }
        wolfModel.GetChild(0).transform.position -= Vector3.up * (transform.position.y - lastPosition.y);
    }

    //***** Gameflow *****//

    /// <summary>
    /// Check if player was for too much time on light / shadow
    /// </summary>
    private void LightTimeCheck()
    {
        if(!isProtected)
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
                Debug.Log("Die: " + isWhite);
                GameManager.Instance.WolfDeath(isWhite);
            }
        }
    }

    /// <summary>
    /// Protect/unprotect the player from light/shadow
    /// </summary>
    /// <param name="isSafe">If the player enter or exit a safe area</param>
    public void ProtectedArea(bool isSafe)
    {
        isProtected = isSafe;
        //Reset light/shadow exposure timer
        if (isSafe)
            lightTime = 0;

        //Update particle systems state
        UpdateLightDamageParticles();
    }

    /// <summary>
    /// Kill the player and respawn
    /// </summary>
    public void ReceiveDamage()
    {
        GameManager.Instance.WolfDeath(isWhite);
    }

    /// <summary>
    /// Update light exposure state and turn on/off particles
    /// </summary>
    /// <param name="newIsInLight"></param>
    public void ChangeLightExposition(bool newIsInLight)
    {
        if(isInLight != newIsInLight)
        {
            isInLight = newIsInLight;
            UpdateLightDamageParticles();
        }
    }

    /// <summary>
    /// Check if damage particles should be displayed or not and update its state
    /// </summary>
    private void UpdateLightDamageParticles()
    {
        if ((isInLight && !isWhite) || (!isInLight && isWhite))
        {
            if (isProtected)
                lightDamagePaticles.StopSystems();
            else
                lightDamagePaticles.PlaySystems();
        }
        else
        {
            lightDamagePaticles.StopSystems();
        }
    }

    #endregion
}
