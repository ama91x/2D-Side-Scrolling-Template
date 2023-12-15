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
[RequireComponent(typeof(PlayerAbilityManager))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(JumpEvent))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(Health))]

[DisallowMultipleComponent]
#endregion
public class Player : MonoBehaviour
{
    // Inspector Assigne
    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();

    // Private
    private CustomPhysics _customPhysics;
    private PlayerController _playerController;
    private Health _health;
    private PlayerAbilityManager _playerAbilityManager;

    private IdleEvent _idleEvent;
    private JumpEvent _jumpEvent;
    private MovementByVelocityEvent _movementByVelocityEvent;
    private MovementToPositionEvent _movementToPositionEvent;
    private SetActiveWeaponEvent _setActiveWeaponevent;
    private ActiveWeapon _activeWeapon;
    private FireWeaponEvent _fireWeaponEvent;
    private WeaponFiredEvent _weaponFiredEvent;

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

    public PlayerAbilityManager PlayerAbilityManagers
    {
        get => _playerAbilityManager;
    }

    public List<Weapon> WeaponList
    {
        get => _weaponList;
    }

    public SetActiveWeaponEvent SetActiveWeaponEvents
    {
        get => _setActiveWeaponevent;
        set => _setActiveWeaponevent = value;
    }

    public ActiveWeapon ActiveWeapons
    {
        get => _activeWeapon;
    }

    public FireWeaponEvent FireWeaponEvents
    {
        get => _fireWeaponEvent;
    }

    public WeaponFiredEvent WeaponFiredEvents
    {
        get => _weaponFiredEvent;
    }

    private void Awake()
    {
        _customPhysics = GetComponent<CustomPhysics>();
        _playerController = GetComponent<PlayerController>();
        _health = GetComponent<Health>();
        _playerAbilityManager = GetComponent<PlayerAbilityManager>();

        _idleEvent = GetComponent<IdleEvent>();
        _jumpEvent = GetComponent<JumpEvent>();
        _movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        _movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        _setActiveWeaponevent = GetComponent<SetActiveWeaponEvent>();
        _activeWeapon = GetComponent<ActiveWeapon>();
        _fireWeaponEvent = GetComponent<FireWeaponEvent>();
        _weaponFiredEvent = GetComponent<WeaponFiredEvent>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();

    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this._playerDetails = playerDetails;

        CreatePlayerStartingWeapons();

        SetPlayerHealth();
    }

    private void CreatePlayerStartingWeapons()
    {
        _weaponList.Clear();

        foreach (WeaponDetailsSO weaponDetails in _playerDetails.StartingWeaponList)
        {
            AddWeaponToPlayer(weaponDetails);
        }
    }

    private void SetPlayerHealth()
    {
        _health.SetStartingHealth(_playerDetails.PlayerHealthAmount);
    }

    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        Weapon weapon = new Weapon()
        {
            WeaponDetails = weaponDetails,
            WeaponReloadTime = 0.015f,
            WeaponClipRemainingAmmo = weaponDetails.WeaponClipAmmoCapacity,
            WeaponReaminingAmmo = weaponDetails.WeaponAmmoCapacity,
            IsWeaponReloading = false
        };

        _weaponList.Add(weapon);

        weapon.WeaponListPosition = _weaponList.Count;

        _setActiveWeaponevent.CallSetActiveWeaponEvent(weapon);

        return weapon;
    }
}
