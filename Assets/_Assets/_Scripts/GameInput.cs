using System;
using UnityEngine;


public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteraction;
    // takes inputs from PlayerInputActions "player" input action map as vector 2
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Input events
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // ? null conditional operator checks for event subscriber to avoid throwing errors
        OnInteraction?.Invoke(this, EventArgs.Empty); 
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        
        inputVector = inputVector.normalized;

        return inputVector;
    }
    

}
