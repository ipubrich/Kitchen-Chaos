using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings"; // prefs for control rebinds
    public static GameInput Instance { get; private set; } // singleton for pause etc
    public event EventHandler OnInteraction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding
    {
        // Abstract controls for OptionsUI
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause,
    }

    // takes inputs from PlayerInputActions "player" input action map as vector 2
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;

        // Check for saved data before enabling action map

        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

       // playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Input events need even handlers
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

        // GetBindingText actionmap array output test, helpful for rebinding
        // Debug.Log(GetBindingText(Binding.Interact));

     
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

    public string GetBindingText(Binding binding)
    {
        return binding switch
        {
            Binding.Move_Up => playerInputActions.Player.Move.bindings[1].ToDisplayString(),
            Binding.Move_Down => playerInputActions.Player.Move.bindings[2].ToDisplayString(),
            Binding.Move_Left => playerInputActions.Player.Move.bindings[3].ToDisplayString(),
            Binding.Move_Right => playerInputActions.Player.Move.bindings[4].ToDisplayString(),
            
            Binding.Interact => playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.Interact_Alternate => playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            Binding.Pause => playerInputActions.Player.Pause.bindings[0].ToDisplayString(),
            _ => playerInputActions.Player.Move.bindings[1].ToDisplayString(),
        };
    }

    public void RebindBinding(Binding binding, Action onActionRebound) // Action is a built in delegate takes no parameters and returns void
    {
        // disable action map to allow rebinding
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alternate:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
        //playerInputActions.Player.Move.PerformInteractiveRebinding(1) replaced by above
            .OnComplete(callback => {
                //Debug.Log(callback.action.bindings[1].path);
                //Debug.Log(callback.action.bindings[1].overridePath);
                callback.Dispose(); // memory cleanup
                playerInputActions.Player.Enable();
                onActionRebound();

                Debug.Log(playerInputActions.SaveBindingOverridesAsJson()); // output changes to actionmap as json
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson()); // save binds to PlayerPrefs
                PlayerPrefs.Save();
            })
            .Start(); //starts the rebind process


    }


}
