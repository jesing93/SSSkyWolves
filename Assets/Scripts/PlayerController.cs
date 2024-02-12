using System.Collections;
using System.Collections.Generic;
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

    public void OnMove(Vector2 direction)
    {

    }

    public void OnInteract(Vector2 direction)
    {

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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rel.transform.rotation, rotationSpeed * Time.deltaTime);
    }
}
