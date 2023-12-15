using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

    public void CallFireWeaponEvent(bool fire)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            Fire = fire
        });
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public bool Fire;
}
