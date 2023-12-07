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

    private bool _isFacingRight;
    private bool _isPlayerDashing = false;
    private bool _isPlayerSliding = false;

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
    }

    private void Update()
    {

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
        }

        MovementInput();
        PlayerDashCooldownTimer();

        _velocity.y += _player.CPhsics2D.Gravity * Time.deltaTime;

        _player.CPhsics2D.Move(_velocity * Time.deltaTime);
    }

    private void MovementInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = Input.GetAxisRaw("Vertical");

        bool dashButtonDown = Input.GetKeyDown(KeyCode.LeftControl);
        bool slideButtonDown = Input.GetKeyDown(KeyCode.C);

        float directionX = Mathf.Sign(horizontalMovement);

        Vector2 direction = new Vector2(horizontalMovement, VerticalMovement);

        if (direction != Vector2.zero)
        {
            if (!dashButtonDown && !slideButtonDown)
            {
                _player.MovementByVelocityEvents.CallMovementByVelocityEvent(direction, _moveSpeed * Time.deltaTime);
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
        }

        //Debug.Log(direction);
    }

    private void PlayerDash(Vector3 direction)
    {
        _playerDashCoroutine = StartCoroutine(PlayerDashRoutin(direction));
    }

    private void PlayerSlid(Vector3 direction)
    {
        _playerSlideCotoutine = StartCoroutine(PlayerSlideRoutin(direction));
    }

    private IEnumerator PlayerDashRoutin(Vector3 direction)
    {
        float minDistance = 0.2f;

        _isPlayerDashing = true;

        Vector3 targetPosition = _player.transform.position + direction * _movementDetails.DashDistance;

        while (Vector3.Distance(_player.transform.position, targetPosition) > minDistance)
        {
            _player.MovementToPositionEvents.CallMovementToPositionEvent(targetPosition, _player.transform.position, _movementDetails.DashSpeed * Time.deltaTime, direction, _isPlayerDashing, false);

            Debug.Log("Player Distance is: " + Vector3.Distance(_player.transform.position, targetPosition));

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

        Vector3 targetPosition = _player.transform.position + direction * _movementDetails.SlideDistance;

        while (Vector3.Distance(_player.transform.position, targetPosition) > minDistance)
        {
            _player.MovementToPositionEvents.CallMovementToPositionEvent(targetPosition, _player.transform.position, _movementDetails.SlideSpeed * Time.deltaTime, direction, false, _isPlayerSliding);

            Debug.Log("Player Distance is: " + Vector3.Distance(_player.transform.position, targetPosition));

            yield return _waitForFixedUpdate;
        }

        _isPlayerSliding = false;
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

            _isPlayerSliding = false;
        }
    }
}
