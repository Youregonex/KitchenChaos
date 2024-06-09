using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private const float ROTATION_SPEED = 10f;
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private ClearCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += _gameInput_OnInteractAction;
    }

    private void _gameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
            _selectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,
                                            transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            moveDirection,
                                            moveDistance);

        if (!canMove)
        {
            // Can't move towards moveDirection

            // Attempt only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position,
                               transform.position + Vector3.up * playerHeight,
                               playerRadius,
                               moveDirectionX,
                               moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDirection = moveDirectionX;
            }
            else
            {
                // Can't move only on the X

                // Attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position,
                                   transform.position + Vector3.up * playerHeight,
                                   playerRadius,
                                   moveDirectionZ,
                                   moveDistance);
                if (canMove)
                {
                    // Can move only on the Z
                    moveDirection = moveDirectionZ;
                }
            }

        }

        if (canMove)
        {
            transform.position += moveDistance * moveDirection;
        }

        _isWalking = moveDirection != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * ROTATION_SPEED);
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
            _lastInteractDirection = moveDirection;

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance, _counterLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter)
                    SetSelectedCounter(clearCounter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = _selectedCounter
        });
    }

    public bool IsWalking() => _isWalking;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() => _kitchenObject != null ? true : false;

    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;
}
