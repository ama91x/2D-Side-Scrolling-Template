using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionEventArgs> OnMoveToPosition;

    public void CallMovementToPositionEvent(Vector3 movePosition, Vector3 currentPosition, float moveSpeed, Vector2 moveDirection, bool IsDashing = false)
    {
        OnMoveToPosition?.Invoke(this, new MovementToPositionEventArgs()
        {
            MovePosition = movePosition,
            CurrentPosition = currentPosition,
            MoveSpeed = moveSpeed,
            MoveDirection = moveDirection,
            IsDashing = IsDashing
        });
    }
}

public class MovementToPositionEventArgs : EventArgs
{
    public Vector3 MovePosition;
    public Vector3 CurrentPosition;
    public Vector2 MoveDirection;
    public float MoveSpeed;
    public bool IsDashing;
}
