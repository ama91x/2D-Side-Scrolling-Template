using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponFiredEvent : MonoBehaviour
{
    public event Action<WeaponFiredEvent, WeaponFiredEventArgs> OnWeaponFired;

    public void CallWeaponFiredEvent(Weapon weapon)
    {
        OnWeaponFired?.Invoke(this, new WeaponFiredEventArgs()
        {
            Weapons = weapon
        });
    }
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon Weapons;
}
