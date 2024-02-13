using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementTensor;
    [SerializeField] private float movementDeath;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool calculateWithSine;
    private float xInput;
    private float zInput;

    [Header("Adaptation")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float step;
    [SerializeField] private float adaptationSpeed;

    [Header("Data")]
    [SerializeField] private PlayerCameraData cameraData;

    // References
    private Rigidbody rb;
    private PlayerAnimator panim;
    private Animator anim;
    private GameObject rel;
    private PlayerCamera cam;

    void Start()
    {
        GetReferences();
    }
    private void Update()
    {
        GetInputs();
        AdaptToTheTerrain();
    }
    void FixedUpdate()
    {
        Move();
    }
    void LateUpdate()
    {
        panim.speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
    }

    private void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
        panim = GetComponent<PlayerAnimator>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rel = new GameObject();
        cam = new GameObject().AddComponent<PlayerCamera>();
        cam.SetUp(this.transform, cameraData);
    }

    private void GetInputs()
    {
        if (Input.GetKey(KeyCode.W)){
            zInput = Mathf.MoveTowards(zInput, 1, Time.deltaTime * movementTensor);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            zInput = Mathf.MoveTowards(zInput, -1, Time.deltaTime * movementTensor);
        }
        else
        {
            zInput = Mathf.MoveTowards(zInput, 0, Time.deltaTime * movementDeath);
        }
        if (Input.GetKey(KeyCode.D))
        {
            xInput = Mathf.MoveTowards(xInput, 1, Time.deltaTime * movementTensor);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xInput = Mathf.MoveTowards(xInput, -1, Time.deltaTime * movementTensor);
        }
        else
        {
            xInput = Mathf.MoveTowards(xInput, 0, Time.deltaTime * movementDeath);
        }
    }
    private void Move()
    {
        Vector3 direction;
        if (calculateWithSine)
            direction = new((Mathf.Sin(xInput * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(xInput), 0, (Mathf.Sin(zInput * 3.14f - 3.14f / 2) / 2 + 0.5f) * Mathf.Sign(zInput));
        else
            direction = new(xInput, 0, zInput);

        if(direction.magnitude > 1) direction.Normalize();
        direction *= movementSpeed;

        rel.transform.position = transform.position;
        rel.transform.LookAt(transform.position + direction);

        direction.y = rb.velocity.y;
        rb.velocity = direction;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, rel.transform.eulerAngles.y, transform.eulerAngles.z)), rotationSpeed * Time.deltaTime);
    }

    private void AdaptToTheTerrain()
    {
        Ray forwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0,step,0.65f)), Vector3.down);
        RaycastHit forwardHit;
        Ray backwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0,step,-0.65f)), Vector3.down);
        RaycastHit backwardHit;

        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0, step, 0.65f)), Vector3.down, Color.red);
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0, step, -0.65f)), Vector3.down, Color.red);

        Vector3 fPoint = forwardSensor.GetPoint(step);
        Vector3 bPoint = forwardSensor.GetPoint(step);

        if(Physics.Raycast(forwardSensor, out forwardHit, step * 2,  groundLayer) && forwardHit.distance > 0.01f)
        {
            fPoint = forwardHit.point;
        }
        if(Physics.Raycast(backwardSensor, out backwardHit, step * 2, groundLayer) && backwardHit.distance > 0.01f)
        {
            bPoint = backwardHit.point;
        }

        rel.transform.position = bPoint;
        rel.transform.LookAt(fPoint);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(rel.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)), adaptationSpeed * Time.deltaTime);
    }
}
