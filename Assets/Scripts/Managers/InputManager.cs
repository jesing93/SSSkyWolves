using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput input;
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = new();
        //Assign the controls to the player depending on the color selected
        //TODO: Assign the selected controls to the player instead of the default
        if (controller.isWhite)
        {
            input.Player.MoveWASD.performed += ctx => controller.OnMove(ctx.ReadValue<Vector2>());
            input.Player.MoveWASD.canceled += ctx => controller.OnMove(Vector2.zero);
            input.Player.InteractWASD.started += ctx => controller.OnInteract(ctx.ReadValue<Vector2>());
            input.Player.SkillWASD.started += ctx => Debug.Log("Skill"); //TODO: Call the skill manager
        }
        else
        {
            input.Player.MoveIKJL.performed += ctx => controller.OnMove(ctx.ReadValue<Vector2>());
            input.Player.MoveIKJL.canceled += ctx => controller.OnMove(Vector2.zero);
            input.Player.InteractIKJL.started += ctx => controller.OnInteract(ctx.ReadValue<Vector2>());
            input.Player.SkillIKJL.started += ctx => Debug.Log("Skill"); //TODO: Call the skill manager
        }
        input.Player.Pause.started += ctx => Debug.Log("Pause"); //TODO: Move to the menu manager

        //If player input not enabled: enable
        if (!input.Player.enabled)
            input.Player.Enable();
    }
}
