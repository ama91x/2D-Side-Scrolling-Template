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
        _player.MovementByVelocityEvents.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        _player.IdleEvents.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable()
    {
        _player.MovementByVelocityEvents.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        _player.IdleEvents.OnIdle -= IdleEvent_OnIdle;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        SetMovementAnimationParameters();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdealAnimationsParameters();
    }

    private void SetMovementAnimationParameters()
    {
        _player.Animators.SetBool(Settings.IsMoving, true);
        _player.Animators.SetBool(Settings.IsIdle, false);
        _player.Animators.SetBool(Settings.IsAiming, false);
    }

    private void SetIdealAnimationsParameters()
    {
        _player.Animators.SetBool(Settings.IsMoving, false);
        _player.Animators.SetBool(Settings.IsIdle, true);
    }
}
