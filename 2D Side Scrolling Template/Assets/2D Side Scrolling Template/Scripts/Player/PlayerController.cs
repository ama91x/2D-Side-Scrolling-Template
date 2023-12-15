using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Inspector Assigne
    [SerializeField] private MovementDetailsSO _movementDetails;

    // Private
    private int _currentWeaponIndex = 1;

    private float _moveSpeed;
    private float _playerDashCoolDownTimer = 0.0f;
    private float _playerSlideCoolDownTimer = 0.0f;
    private float _currentSpeed;
    private float _maxJumpVelocity;
    private float _minJumpVelocity;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    private bool _isFacingRight;
    private bool _isPlayerDashing = false;
    private bool _isPlayerSliding = false;
    private bool _isPlayerFinishSlide = false;
    private bool _jumpBuffered;

    private Vector3 _velocity;

    private Coroutine _playerDashCoroutine;
    private Coroutine _playerSlideCotoutine;
    private WaitForFixedUpdate _waitForFixedUpdate;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();

        _moveSpeed = _movementDetails.GetMovementSpeed();
    }

    private void Start()
    {
        _waitForFixedUpdate = new WaitForFixedUpdate();

        SetStartingWeapon();

        _player.CPhsics2D.Gravity = -(2 * _movementDetails.MaxJumpHeight) / Mathf.Pow(_movementDetails.TimeToJumpApex, 2);

        _maxJumpVelocity = Mathf.Abs(_player.CPhsics2D.Gravity) * _movementDetails.TimeToJumpApex;
        _minJumpVelocity = MathF.Sqrt(2 * Mathf.Abs(_player.CPhsics2D.Gravity) * _movementDetails.MinJumpHeight);

        Debug.Log("Gravity is: " + _player.CPhsics2D.Gravity + "Jump Velcoity: " + _maxJumpVelocity);
    }

    private void Update()
    {

        if (_isPlayerFinishSlide)
        {
            _player.CPhsics2D.CalculateRaySpacing();
        }

        if (_isPlayerDashing || _isPlayerSliding)
        {
            if (_player.CPhsics2D.CollisionInfos.Left || _player.CPhsics2D.CollisionInfos.Right)
            {
                StopPlayerDashRoutine();
                StopPlayerSlideRoutine();
            }

            return;
        }

        if (_player.CPhsics2D.CollisionInfos.Below || _player.CPhsics2D.CollisionInfos.Above)
        {
            _velocity.y = 0;
        }

        if (_player.CPhsics2D.CollisionInfos.Below)
        {
            _coyoteTimeCounter = _movementDetails.CoyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (_jumpBuffered)
        {
            _jumpBufferCounter -= Time.deltaTime;
            if (_jumpBufferCounter <= 0)
            {
                _jumpBuffered = false;
            }
        }

        PlayerJump();
        MovementInput();
        WeaponInput();
        PlayerDashCooldownTimer();


        if (_velocity.y < 0)
        {
            _velocity.y += _player.CPhsics2D.Gravity * _player.CPhsics2D.GravityMultiplier * Time.deltaTime;
        }
        else
        {
            _velocity.y += _player.CPhsics2D.Gravity * Time.deltaTime;
        }

        _player.CPhsics2D.Move(_velocity * Time.deltaTime);
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in _player.WeaponList)
        {
            if (weapon.WeaponDetails == _player.PlayerDetails.StartingWeapon)
            {
                SetWeaponIndex(index);
                break;
            }
            index++;
        }
    }

    private void MovementInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");

        bool dashButtonDown = Input.GetKeyDown(KeyCode.LeftControl);
        bool slideButtonDown = Input.GetKeyDown(KeyCode.C);

        float directionX = Mathf.Sign(horizontalMovement);

        Vector2 direction = new Vector2(horizontalMovement, 0.0f);

        if (direction != Vector2.zero)
        {
            if (!dashButtonDown && !slideButtonDown)
            {
                _currentSpeed += _movementDetails.Acceleration;

                if (_currentSpeed > _moveSpeed)
                {
                    _currentSpeed = _moveSpeed;
                }
                _player.MovementByVelocityEvents.CallMovementByVelocityEvent(direction, _moveSpeed * Time.deltaTime);

                _isPlayerFinishSlide = false;
            }
            else if (_playerDashCoolDownTimer <= 0.0f && !slideButtonDown && _player.PlayerAbilityManagers.CanDash)
            {
                PlayerDash((Vector3)direction);
            }
            else if (_playerDashCoolDownTimer <= 0.0f && !dashButtonDown && _player.PlayerAbilityManagers.CanSlide)
            {
                PlayerSlid((Vector3)direction);
            }

            if (directionX > 0 && !_isFacingRight)
            {
                Turn();
            }
            else if (directionX < 0 && _isFacingRight)
            {
                Turn();
            }
        }
        else
        {
            _player.IdleEvents.CallIdleEvent();
            _currentSpeed = 0;
        }
    }

    private void WeaponInput()
    {
        FireWeaponInput();
    }

    private void FireWeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.FireWeaponEvents.CallFireWeaponEvent(true);
        }
    }

    private void PlayerDash(Vector3 direction)
    {
        _playerDashCoroutine = StartCoroutine(PlayerDashRoutin(direction));
    }

    private void PlayerSlid(Vector3 direction)
    {
        _playerSlideCotoutine = StartCoroutine(PlayerSlideRoutin(direction));
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_coyoteTimeCounter > 0)
            {
                _velocity.y = _player.JumpEvents.CallJumpEvent(_maxJumpVelocity);
                _jumpBuffered = false;
            }
            else
            {
                _jumpBuffered = true;
                _jumpBufferCounter = _movementDetails.JumpBufferLength;
            }
        }

        if (_jumpBuffered && _coyoteTimeCounter > 0)
        {
            _velocity.y = _player.JumpEvents.CallJumpEvent(_maxJumpVelocity);
            _jumpBuffered = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_velocity.y > _minJumpVelocity)
            {
                _velocity.y = _player.JumpEvents.CallJumpEvent(_minJumpVelocity);
            }
        }

        // if (_velocity.y < 0)
        // {
        //     _velocity.y *= _player.CPhsics2D.FallMultiplier;

        //     float maxFallSpeed = _player.CPhsics2D.Gravity / 1.6f;

        //     if (_velocity.y < maxFallSpeed)
        //     {
        //         _velocity.y = maxFallSpeed;
        //     }
        // }
    }

    private IEnumerator PlayerDashRoutin(Vector3 direction)
    {
        float minDistance = 0.2f;

        _isPlayerDashing = true;

        Vector3 targetPosition = _player.transform.position + direction * _movementDetails.DashDistance;

        while (Vector3.Distance(_player.transform.position, targetPosition) > minDistance)
        {
            _player.MovementToPositionEvents.CallMovementToPositionEvent(targetPosition, _player.transform.position, _movementDetails.DashSpeed * Time.deltaTime, direction, _isPlayerDashing, false);

            // Debug.Log("Player Distance is: " + Vector3.Distance(_player.transform.position, targetPosition));

            yield return _waitForFixedUpdate;
        }

        _isPlayerDashing = false;
        _playerDashCoolDownTimer = _movementDetails.DashCoolDown;

        _player.transform.position = targetPosition;
    }

    private IEnumerator PlayerSlideRoutin(Vector3 direction)
    {
        float minDistance = 0.2f;

        _isPlayerSliding = true;
        _isPlayerFinishSlide = false;

        Vector3 targetPosition = _player.transform.position + direction * _movementDetails.SlideDistance;

        while (Vector3.Distance(_player.transform.position, targetPosition) > minDistance)
        {
            _player.MovementToPositionEvents.CallMovementToPositionEvent(targetPosition, _player.transform.position, _movementDetails.SlideSpeed * Time.deltaTime, direction, false, _isPlayerSliding);

            _player.CPhsics2D.CalculateRaySpacing();

            yield return _waitForFixedUpdate;
        }

        _isPlayerSliding = false;
        _isPlayerFinishSlide = true;

        _playerSlideCoolDownTimer = _movementDetails.SlideCoolDown;

        _player.transform.position = targetPosition;
    }

    private void PlayerDashCooldownTimer()
    {
        if (_playerDashCoolDownTimer >= 0.0f)
        {
            _playerDashCoolDownTimer -= Time.deltaTime;
        }
    }

    private void SetWeaponIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < _player.WeaponList.Count)
        {
            _currentWeaponIndex = weaponIndex;
            _player.SetActiveWeaponEvents.CallSetActiveWeaponEvent(_player.WeaponList[weaponIndex - 1]);
        }
    }

    private void Turn()
    {
        if (_isFacingRight)
        {
            // Rotating the sprite 180 degree
            // Vector3 rotate = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            // transform.rotation = Quaternion.Euler(rotate);
            // _player.BoxCollider.size = new Vector2(-0.6f, 1.3f);

            // Fliping The sprite by local Scale
            _player.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            _isFacingRight = !_isFacingRight;
        }
        else
        {
            // Rotating the sprite 180 degree
            // Vector3 rotate = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            // transform.rotation = Quaternion.Euler(rotate);
            // _player.BoxCollider.size = new Vector2(0.6f, 1.3f);

            // Fliping The sprite by local Scale
            _player.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            _isFacingRight = !_isFacingRight;
        }
    }

    private void StopPlayerDashRoutine()
    {
        if (_playerDashCoroutine != null)
        {
            StopCoroutine(_playerDashCoroutine);

            _isPlayerDashing = false;
        }
    }

    private void StopPlayerSlideRoutine()
    {
        if (_playerSlideCotoutine != null)
        {
            StopCoroutine(_playerSlideCotoutine);

            _isPlayerFinishSlide = true;
            _isPlayerSliding = false;
        }
    }
}
