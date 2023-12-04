using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour
{
    private IdleEvent _idleEvent;
    private CustomPhysics _cPhysics;

    private void Awake()
    {
        _idleEvent = GetComponent<IdleEvent>();
        _cPhysics = GetComponent<CustomPhysics>();
    }

    private void OnEnable()
    {
        _idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable()
    {
        _idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _cPhysics.Move(new Vector3(0, 0, 0));
    }
}
