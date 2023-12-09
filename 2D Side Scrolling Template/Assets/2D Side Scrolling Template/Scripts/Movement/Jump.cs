using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private JumpEvent _jumpEvent;

    private CustomPhysics _cPhysics;

    private void Awake()
    {
        _jumpEvent = GetComponent<JumpEvent>();
        _cPhysics = GetComponent<CustomPhysics>();
    }

    private void OnEnable()
    {
        _jumpEvent.OnJump += JumpEvent_OnJump;
    }

    private void OnDisable()
    {
        _jumpEvent.OnJump += JumpEvent_OnJump;
    }

    private void JumpEvent_OnJump(JumpEvent jumpEvent, JumpEventArgs jumpEventArgs)
    {
        JumpPlayer(jumpEventArgs.JumpVelocity);
    }

    private void JumpPlayer(float jumpVelcoity)
    {
        _cPhysics.Move(new Vector3(0, jumpVelcoity * Time.deltaTime, 0));
    }
}
