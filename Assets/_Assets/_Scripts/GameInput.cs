using System;
using UnityEngine;


public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; } // singleton for pause etc
    public event EventHandler OnInteraction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;


    // takes inputs from PlayerInputActions "player" input action map as vector 2
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Input events need even handlers
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        // unsubscribe from events when destroyed
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        // destroy / dispose of object when no longer needed
        playerInputActions.Dispose(); 
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // ? null conditional operator checks for event subscriber to avoid throwing errors
        OnInteraction?.Invoke(this, EventArgs.Empty); 
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        
        inputVector = inputVector.normalized;

        return inputVector;
    }
    

}
