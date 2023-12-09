using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region REQUIRED COMPONENTS
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(JumpEvent))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(Health))]

[DisallowMultipleComponent]
#endregion
public class Player : MonoBehaviour
{
    // Private
    private CustomPhysics _customPhysics;
    private PlayerController _playerController;
    private Health _health;

    private IdleEvent _idleEvent;
    private JumpEvent _jumpEvent;
    private MovementByVelocityEvent _movementByVelocityEvent;
    private MovementToPositionEvent _movementToPositionEvent;

    private PlayerDetailsSO _playerDetails;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private BoxCollider2D _boxCollider;

    // Accessors & Mutators
    public Health Healths
    {
        get => _health;
    }

    public PlayerDetailsSO PlayerDetails
    {
        get => _playerDetails;
    }

    public BoxCollider2D BoxCollider
    {
        get => _boxCollider;

        set
        {
            _boxCollider = value;
        }
    }

    public CustomPhysics CPhsics2D
    {
        get => _customPhysics;
    }

    public Animator Animators
    {
        get => _animator;
    }

    public MovementByVelocityEvent MovementByVelocityEvents
    {
        get => _movementByVelocityEvent;
    }

    public IdleEvent IdleEvents
    {
        get => _idleEvent;
    }

    public JumpEvent JumpEvents
    {
        get => _jumpEvent;
    }

    public MovementToPositionEvent MovementToPositionEvents
    {
        get => _movementToPositionEvent;
    }

    private void Awake()
    {
        _customPhysics = GetComponent<CustomPhysics>();
        _playerController = GetComponent<PlayerController>();
        _health = GetComponent<Health>();

        _idleEvent = GetComponent<IdleEvent>();
        _jumpEvent = GetComponent<JumpEvent>();
        _movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        _movementToPositionEvent = GetComponent<MovementToPositionEvent>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();

    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this._playerDetails = playerDetails;

        SetPlayerHealth();
    }

    private void SetPlayerHealth()
    {
        _health.SetStartingHealth(_playerDetails.PlayerHealthAmount);
    }
}
