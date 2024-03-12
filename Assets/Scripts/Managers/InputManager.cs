using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInput input;
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = new();
    }

    private void Start()
    {
        //Assign the controls to the player depending on the color selected
        if (controller.isWhite)
        {
            input.Player.MoveWASD.performed += ctx => controller.OnMove(ctx.ReadValue<Vector2>());
            input.Player.MoveWASD.canceled += ctx => controller.OnMove(Vector2.zero);
            input.Player.InteractWASD.started += ctx => controller.OnInteract();
            input.Player.SkillWASD.started += ctx => Debug.Log("Skill"); //TODO: Call the skill manager
            input.Player.Pause.started += ctx => GameManager.Instance.TogglePause();
            input.UI.Pause.started += ctx => GameManager.Instance.TogglePause();
            input.UI.Cancel.started += ctx => GameManager.Instance.TogglePause(); //TODO: Call the menu manager
        }
        else
        {
            input.Player.MoveIKJL.performed += ctx => controller.OnMove(ctx.ReadValue<Vector2>());
            input.Player.MoveIKJL.canceled += ctx => controller.OnMove(Vector2.zero);
            input.Player.InteractIKJL.started += ctx => controller.OnInteract();
            input.Player.SkillIKJL.started += ctx => Debug.Log("Skill"); //TODO: Call the skill manager
        }

        //Only enable player input
        if (input.UI.enabled)
            input.UI.Disable();
        if (!input.Player.enabled)
            input.Player.Enable();
    }

    /// <summary>
    /// Toggle inputs between Game and UI
    /// </summary>
    /// <param name="isGamePaused">True if the game was paused</param>
    public void TogglePause(bool isGamePaused)
    {
        if(isGamePaused)
        {
            SwitchInputMode(1);
        }
        else
        {
            SwitchInputMode(0);
        }
    }

    /// <summary>
    /// Switch between input modes. 0 is Game and 1 is UI
    /// </summary>
    /// <param name="mode"></param>
    private void SwitchInputMode(int mode)
    {
        switch (mode)
        {
            case 0: //Switch to Game control
                input.UI.Disable();
                input.Player.Enable();
                break;
            case 1: //Switch to UI control
                input.Player.Disable();
                input.UI.Enable();
                break;
        }
    }
}
