using UnityEngine;
using System;
using Unity.Netcode;
using System.Collections.Generic;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static Player LocalInstance { get; private set; }

    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private LayerMask _collisionsLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;
    [SerializeField] private List<Vector3> _spawnPositionList;

    private const float ROTATION_SPEED = 10f;
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPickedSomething = null;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            LocalInstance = this;
        }

        transform.position = _spawnPositionList[(int)OwnerClientId];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
            return;

        if (_selectedCounter != null)
            _selectedCounter.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
            return;

        if (_selectedCounter != null)
            _selectedCounter.Interact(this);
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        bool canMove = !Physics.BoxCast(transform.position,
                                        Vector3.one * playerRadius,
                                        moveDirection,
                                        Quaternion.identity,
                                        moveDistance,
                                        _collisionsLayerMask);

        if (!canMove)
        {
            // Can't move towards moveDirection

            // Attempt only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = (moveDirection.x < -.5f || moveDirection.x > +.5f) && !Physics.BoxCast(transform.position,
                                                                                             Vector3.one * playerRadius,
                                                                                             moveDirectionX,
                                                                                             Quaternion.identity,
                                                                                             moveDistance,
                                                                                             _collisionsLayerMask);

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
                canMove = (moveDirection.z < -.5f || moveDirection.z > +.5f) && !Physics.BoxCast(transform.position,
                                                                                                 Vector3.one * playerRadius,
                                                                                                 moveDirectionZ,
                                                                                                 Quaternion.identity,
                                                                                                 moveDistance,
                                                                                                 _collisionsLayerMask);
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
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
            _lastInteractDirection = moveDirection;

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance, _counterLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                    SetSelectedCounter(baseCounter);
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

    private void SetSelectedCounter(BaseCounter selectedCounter)
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

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }


    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() => _kitchenObject != null ? true : false;

    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;
}
