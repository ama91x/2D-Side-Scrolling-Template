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

    private Vector3 _velocity;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();

        _moveSpeed = _movementDetails.GetMovementSpeed();
    }

    private void Update()
    {
        MovementInput();

        _velocity.y += _player.CPhsics2D.Gravity * Time.deltaTime;

        _player.CPhsics2D.Move(_velocity * Time.deltaTime);
    }

    private void MovementInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");

        Vector2 direction = new Vector2(horizontalMovement, 0);

        if (direction != Vector2.zero)
        {
            _player.MovementByVelocityEvents.CallMovementByVelocityEvent(direction, _moveSpeed * Time.deltaTime);
        }
        else
        {
            _player.IdleEvents.CallIdleEvent();
        }

        Debug.Log(direction);
    }
}
