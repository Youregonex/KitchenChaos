using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
    }

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;


    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_Performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
        _playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= Interact_Performed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_Performed;
        _playerInputActions.Player.Pause.performed -= Pause_performed;

        _playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch(binding)
        {
            default:
            case Binding.Move_Up:
                return _playerInputActions.Player.Move.bindings[1].ToDisplayString();

            case Binding.Move_Down:
                return _playerInputActions.Player.Move.bindings[2].ToDisplayString();

            case Binding.Move_Left:
                return _playerInputActions.Player.Move.bindings[3].ToDisplayString();

            case Binding.Move_Right:
                return _playerInputActions.Player.Move.bindings[4].ToDisplayString();

            case Binding.Interact:
                return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();

            case Binding.InteractAlternate:
                return _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();

            case Binding.Pause:
                return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch(binding)
        {
            default:
            case Binding.Move_Up:

                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;

                break;

            case Binding.Move_Down:

                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;

                break;

            case Binding.Move_Left:

                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;

                break;

            case Binding.Move_Right:

                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;

                break;

            case Binding.Interact:

                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;

                break;

            case Binding.InteractAlternate:

                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;

                break;

            case Binding.Pause:

                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;

                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            _playerInputActions.Player.Enable();
            onActionRebound();

        }).Start();

    }

}
