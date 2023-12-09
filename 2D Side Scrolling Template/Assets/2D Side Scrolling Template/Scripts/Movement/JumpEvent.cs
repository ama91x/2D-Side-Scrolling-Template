using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEvent : MonoBehaviour
{
    public event Action<JumpEvent, JumpEventArgs> OnJump;

    public float CallJumpEvent(float jumpVelcoity)
    {
        OnJump?.Invoke(this, new JumpEventArgs()
        {
            JumpVelocity = jumpVelcoity
        });

        return jumpVelcoity;
    }
}

public class JumpEventArgs : EventArgs
{
    public float JumpVelocity;
}