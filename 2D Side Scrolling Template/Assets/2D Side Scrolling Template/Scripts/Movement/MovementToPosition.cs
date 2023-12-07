using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementToPosition : MonoBehaviour
{
    private CustomPhysics _cPhysics;
    private MovementToPositionEvent _movmentToPositionEvent;

    private Player _player;

    private void Awake()
    {
        _cPhysics = GetComponent<CustomPhysics>();
        _movmentToPositionEvent = GetComponent<MovementToPositionEvent>();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _movmentToPositionEvent.OnMoveToPosition += MovementToPositionEvent_OnMoveToPosition;
    }

    private void OnDisable()
    {
        _movmentToPositionEvent.OnMoveToPosition -= MovementToPositionEvent_OnMoveToPosition;
    }

    private void MovementToPositionEvent_OnMoveToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionEventArgs movementToPositionEventArgs)
    {
        MovePlayer(movementToPositionEventArgs.MovePosition, movementToPositionEventArgs.CurrentPosition, movementToPositionEventArgs.MoveSpeed);
    }

    private void MovePlayer(Vector3 movePosition, Vector3 currentPosition, float moveSpeed)
    {
        Vector3 unitVector = Vector3.Normalize(movePosition - currentPosition);
        _cPhysics.Move(unitVector * moveSpeed);
    }
}
