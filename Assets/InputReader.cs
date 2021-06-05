using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private VerticalMovement playerVerticalMovement;
    [SerializeField] private HorizontalMovement playerHorizontalMovement;
    [SerializeField] private Dash dash;

    private DashAttackUltimateInputs Inputs;

    private void Awake()
    {
        Inputs = new DashAttackUltimateInputs();
    }

    private void Start()
    {
        Inputs.Player.Jump.performed += ctx =>
        {
            playerVerticalMovement.Input = true;
            playerHorizontalMovement.JumpInput = true;
        };

        Inputs.Player.CancelJump.performed += ctx =>
        {
            playerVerticalMovement.Input = false;
            playerHorizontalMovement.JumpInput = false;
        };

        Inputs.Player.Dash.performed += ctx => dash.Input = true;
        Inputs.Player.Dash.canceled += ctx => dash.Input = false;
    }

    private void Update()
    {
        playerHorizontalMovement.Input = Inputs.Player.Move.ReadValue<float>();
        dash.Direction = Inputs.Player.DashDirection.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        Inputs.Enable();
    }
}