using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayer : MonoBehaviour
{
    // Peivate
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _player.IdleEvents.OnIdle += IdleEvent_OnIdle;
        _player.MovementByVelocityEvents.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        _player.MovementToPositionEvents.OnMoveToPosition += MovementToPositionEvent_OnMoveToPosition;
    }

    private void OnDisable()
    {
        _player.IdleEvents.OnIdle -= IdleEvent_OnIdle;
        _player.MovementByVelocityEvents.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        _player.MovementToPositionEvents.OnMoveToPosition -= MovementToPositionEvent_OnMoveToPosition;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        InitializeDashAnimationParameters();
        InitializeSlidingAnimationParameters();
        SetMovementAnimationParameters();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        InitializeDashAnimationParameters();
        InitializeSlidingAnimationParameters();
        SetIdealAnimationsParameters();
    }

    private void MovementToPositionEvent_OnMoveToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionEventArgs movementToPositionEventArgs)
    {
        InitializeDashAnimationParameters();
        InitializeSlidingAnimationParameters();
        SetMovementToPositionAnimationParameters(movementToPositionEventArgs);
    }

    private void SetMovementAnimationParameters()
    {
        _player.Animators.SetBool(Settings.IsMoving, true);
        _player.Animators.SetBool(Settings.IsIdle, false);
        _player.Animators.SetBool(Settings.IsAiming, false);
        _player.Animators.SetBool(Settings.IsDashing, false);
    }

    private void SetIdealAnimationsParameters()
    {
        _player.Animators.SetBool(Settings.IsMoving, false);
        _player.Animators.SetBool(Settings.IsIdle, true);
        _player.Animators.SetBool(Settings.IsDashing, false);
    }

    private void InitializeDashAnimationParameters()
    {
        _player.Animators.SetBool(Settings.IsDashing, false);
    }

    private void InitializeSlidingAnimationParameters()
    {
        _player.Animators.SetBool(Settings.IsSliding, false);
    }

    private void SetMovementToPositionAnimationParameters(MovementToPositionEventArgs movementToPositionEventArgs)
    {
        if (movementToPositionEventArgs.IsDashing)
        {
            _player.Animators.SetBool(Settings.IsDashing, true);
            _player.Animators.SetBool(Settings.IsMoving, false);
        }
        else if (movementToPositionEventArgs.IsSliding)
        {
            _player.Animators.SetBool(Settings.IsDashing, false);
            _player.Animators.SetBool(Settings.IsMoving, false);
            _player.Animators.SetBool(Settings.IsSliding, true);
        }
    }
}
