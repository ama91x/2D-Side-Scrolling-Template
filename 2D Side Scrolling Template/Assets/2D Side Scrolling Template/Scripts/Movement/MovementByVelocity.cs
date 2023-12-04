using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByVelocity : MonoBehaviour
{
    private CustomPhysics _cPhysics;
    private MovementByVelocityEvent _movementByVelocityEvent;

    private void Awake()
    {
        _cPhysics = GetComponent<CustomPhysics>();
        _movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
    }

    private void OnEnable()
    {
        _movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void OnDisable()
    {
        _movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        MoveCharacter(movementByVelocityArgs.MoveDirection, movementByVelocityArgs.MoveSpeed);
    }

    private void MoveCharacter(Vector2 moveDirection, float moveSpeed)
    {
        _cPhysics.Move(moveDirection * moveSpeed);
    }
}
