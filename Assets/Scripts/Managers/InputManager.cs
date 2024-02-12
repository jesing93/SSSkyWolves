using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput input;
    private PlayerController controller;
    [SerializeField] private bool isWhite;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = new();
        if(isWhite)
        {
            input.White.Move.performed += ctx => controller.OnMove(ctx.ReadValue<Vector2>());
            input.White.Interact.performed += ctx => controller.OnInteract(ctx.ReadValue<Vector2>());
            input.White.Enable();
        }
        else
        {
            input.Black.Move.performed += ctx => controller.OnMove(ctx.ReadValue<Vector2>());
            input.Black.Interact.performed += ctx => controller.OnInteract(ctx.ReadValue<Vector2>());
            input.Black.Enable();
        }
    }
}
