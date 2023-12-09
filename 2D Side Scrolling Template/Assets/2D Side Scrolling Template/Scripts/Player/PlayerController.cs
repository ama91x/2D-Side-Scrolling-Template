using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Inspector Assigne
    [SerializeField] private Transform _weaponShootingPosition;
    [SerializeField] private MovementDetailsSO _movementDetails;

    // Private
    private float _moveSpeed;
    private float _playerDashCoolDownTimer = 0.0f;
    private float _playerSlideCoolDownTimer = 0.0f;
    private float _currentSpeed;
    private float _jumpVelocity;

    private bool _isFacingRight;
    private bool _isPlayerDashing = false;
    private bool _isPlayerSliding = false;
    private bool _isPlayerFinishSlide = false;

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

        _player.CPhsics2D.Gravity = -(2 * _movementDetails.JumpHeight) / Mathf.Pow(_movementDetails.TimeToJumpApex, 2);

        _jumpVelocity = Mathf.Abs(_player.CPhsics2D.Gravity) * _movementDetails.TimeToJumpApex;

        Debug.Log("Gravity is: " + _player.CPhsics2D.Gravity + "Jump Velcoity: " + _jumpVelocity);
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
                Debug.Log("Collsions Appere");
            }

            return;
        }

        if (_player.CPhsics2D.CollisionInfos.Below || _player.CPhsics2D.CollisionInfos.Above)
        {
            _velocity.y = 0;
            //_player.Animators.SetBool(Settings.IsJumping, false);
        }

        PlayerJump();
        MovementInput();
        PlayerDashCooldownTimer();

        _velocity.y += _player.CPhsics2D.Gravity * Time.deltaTime;

        _player.CPhsics2D.Move(_velocity * Time.deltaTime);
    }

    private void MovementInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        //float VerticalMovement = Input.GetAxisRaw("Vertical");

        bool dashButtonDown = Input.GetKeyDown(KeyCode.LeftControl);
        bool slideButtonDown = Input.GetKeyDown(KeyCode.C);



        float directionX = Mathf.Sign(horizontalMovement);

        Vector2 direction = new Vector2(horizontalMovement, 0);

        if (direction != Vector2.zero)
        {
            if (!dashButtonDown && !slideButtonDown)
            {
                _currentSpeed += _movementDetails.Acceleration;

                if (_currentSpeed > _moveSpeed)
                {
                    _currentSpeed = _moveSpeed;
                }
                _player.MovementByVelocityEvents.CallMovementByVelocityEvent(direction, _currentSpeed * Time.deltaTime);

                _isPlayerFinishSlide = false;
            }
            else if (_playerDashCoolDownTimer <= 0.0f && !slideButtonDown)
            {
                PlayerDash((Vector3)direction);
            }
            else if (_playerDashCoolDownTimer <= 0.0f && !dashButtonDown)
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
        if (Input.GetKeyDown(KeyCode.Space) && _player.CPhsics2D.CollisionInfos.Below)
        {
            _velocity.y = _player.JumpEvents.CallJumpEvent(_jumpVelocity);
        }
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
