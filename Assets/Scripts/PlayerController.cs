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
    private GameObject rel;
    private PlayerCamera cam;

    void Start()
    {
        GetReferences();
    }
    void Update()
    {
        GetInputs();
        Move();
        AdaptToTheTerrain();
    }

    private void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
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
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rel.transform.rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, rel.transform.eulerAngles.y, transform.eulerAngles.z)), rotationSpeed * Time.deltaTime);
    }

    private void AdaptToTheTerrain()
    {
        Ray forwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0,step,0.5f)), Vector3.down);
        RaycastHit forwardHit;
        Ray backwardSensor = new(transform.position + transform.TransformDirection(new Vector3(0,step,-0.5f)), Vector3.down);
        RaycastHit backwardHit;

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
