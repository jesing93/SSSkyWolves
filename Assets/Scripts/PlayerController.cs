using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float gravity;
    private Vector2 moveInputTarget;
    private Vector2 moveInput;
    private float yAxis;
    private Vector2 yAxisCap;

    [Header("Adaptation")]
    [SerializeField] private Vector3 groundSensorsOffset;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float anticipation;
    [SerializeField] private float adaptationStep;
    [SerializeField] private float adaptationSpeed;
    [SerializeField] private Transform wolfModel;

    [Header("Steps")]
    [SerializeField] private Vector3 stepSensorsOffset;
    [SerializeField] private float step;

    [Header("Data")]
    [SerializeField] private PlayerCameraData cameraData;

    [Header("Others")]
    public bool isWhite;

    private bool isInLight;
    float lightTime;
    float maxTimeToDie = 1;

    // References
    private Rigidbody rb;
    private PlayerAnimator panim;
    private Animator anim;
    private GameObject rel;
    private PlayerCamera cam;
    #endregion

    #region Getters / Setters
    public bool IsInLight { get => isInLight; set => isInLight = value; }
    #endregion

    #region Unity Functions
    void Start()
    {
        GetReferences();
    }
    private void Update()
    {
        InputsModifiers();
        
    }
    void FixedUpdate()
    {
        
        Move();
        LightTimeCheck();
        OvercomeStep();

    }
    void LateUpdate()
    {
        CheckGround();
        Adaptation();
        
        panim.Speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        panim.MoveInput = moveInput;
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
    public void OnInteract()
    {
        Debug.Log("OnInteract");
    }
    private void InputsModifiers()
    {
        if(moveInputTarget == Vector2.zero)
            moveInput = Vector2.MoveTowards(moveInput, Vector2.zero, Time.deltaTime * movementDeath);
        else
            moveInput = Vector2.MoveTowards(moveInput, moveInputTarget, Time.deltaTime * movementTensor);
    }

    //***** Movement *****//

    // Comprueba el suelo en las direcciones establecidas y cambia el cap del ejeY según el resultado.
    private void CheckGround()
    {
        Vector3Int[] directions = new Vector3Int[] { new (-1, 1, 1), new(1, 1, 1), new(-1, 1, -1), new(1, 1, -1) };
        float lessDistance = 50;

        for(int i = 0; i < directions.Length; i++)
        {
            Ray groundRay = new Ray(transform.position + transform.TransformDirection(new Vector3(groundSensorsOffset.x * directions[i].x, 0, groundSensorsOffset.z * directions[i].z) + Vector3.up * groundSensorsOffset.y * directions[i].y), Vector3.down);
            RaycastHit groundRayHit;
            if (Physics.Raycast(groundRay, out groundRayHit, adaptationStep, groundLayer))
            {
                if(groundRayHit.distance < lessDistance) lessDistance = groundRayHit.distance;
            }
        }
        if (lessDistance < adaptationStep) yAxisCap.x = 0;
        else yAxisCap.x = -25;
    }

    // Setea la velocidad del rigidbody en función de los ejes de entrada y la gravedad calculada. Además rota al personaje hacia la dirección a la que se mueve.
    private void Move()
    {
        Vector3 direction;
        if (calculateWithSine)
            direction = new((Mathf.Sin(moveInput.x * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(moveInput.x), 0, (Mathf.Sin(moveInput.y * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(moveInput.y));
        else
            direction = new(moveInput.x, 0, moveInput.y);

        if (direction.magnitude > 1) direction.Normalize();
        direction *= movementSpeed;

        rel.transform.position = transform.position;
        rel.transform.LookAt(transform.position + direction);

        yAxis = Mathf.Clamp(yAxis, yAxisCap.x, 25);
        direction.y = yAxis;
        rb.velocity = direction;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, rel.transform.eulerAngles.y, transform.eulerAngles.z)), rotationSpeed * Time.fixedDeltaTime);

        yAxis += gravity * Time.fixedDeltaTime;
    }

    // Calcula la inclinación media del terreno y rota el modelo 3D hasta alcanzarla.
    private void Adaptation()
    {
        Ray forwardSensor = new(transform.position + wolfModel.transform.TransformDirection(new Vector3(0, adaptationStep, groundSensorsOffset.z + anticipation)), wolfModel.transform.TransformDirection(Vector3.down));
        RaycastHit forwardHit;
        Ray backwardSensor = new(transform.position + wolfModel.transform.TransformDirection(new Vector3(0, adaptationStep, -groundSensorsOffset.z + anticipation / 2)), wolfModel.transform.TransformDirection(Vector3.down));
        RaycastHit backwardHit;

        Vector3 fPoint = forwardSensor.GetPoint(adaptationStep);
        fPoint.y = transform.position.y;
        Vector3 bPoint = forwardSensor.GetPoint(adaptationStep);
        bPoint.y = transform.position.y;

        if (Physics.Raycast(forwardSensor, out forwardHit, adaptationStep * 2, groundLayer) && forwardHit.distance > 0.005f)
        {
            fPoint = forwardHit.point;
        }
        if (Physics.Raycast(backwardSensor, out backwardHit, adaptationStep * 2, groundLayer) && backwardHit.distance > 0.005f)
        {
            bPoint = backwardHit.point;
        }

        rel.transform.position = bPoint;
        rel.transform.LookAt(fPoint);

        wolfModel.transform.rotation = Quaternion.RotateTowards(wolfModel.transform.rotation, Quaternion.Euler(new Vector3(rel.transform.eulerAngles.x, wolfModel.transform.eulerAngles.y, wolfModel.transform.eulerAngles.z)), adaptationSpeed * moveInput.magnitude * movementSpeed * Time.deltaTime);
    }

    // Busca escalones o diferencias de altura que pueda sortear, establece la posicion del collider en la más alta y mueve al personaje hacia ella.
    private void OvercomeStep()
    {
        Ray forwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0, 0, stepSensorsOffset.z)) + Vector3.up * step, Vector3.down);
        RaycastHit forwardHit;
        Ray backwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0, 0, -stepSensorsOffset.z)) + Vector3.up * step, Vector3.down);
        RaycastHit backwardHit;

        float greaterHeight;
        Vector3 lastPosition = transform.position;

        if (Physics.Raycast(forwardSensor, out forwardHit, step * 2, groundLayer) && forwardHit.distance > 0.005f && Physics.Raycast(backwardSensor, out backwardHit, step * 2, groundLayer) && backwardHit.distance > 0.005f)
        {
            greaterHeight = Mathf.Max(forwardHit.point.y, backwardHit.point.y);
            Debug.Log(greaterHeight - transform.position.y);
        }
        else {
            greaterHeight = transform.position.y;
        }
        float capsuleOffset = greaterHeight - transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, greaterHeight, transform.position.z), moveInput.magnitude * movementSpeed /2 * Time.fixedDeltaTime);
        transform.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.5f + capsuleOffset, 0);
        //wolfModel.GetChild(0).position -= new Vector3(0,transform.position.y - lastPosition.y,0);
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
