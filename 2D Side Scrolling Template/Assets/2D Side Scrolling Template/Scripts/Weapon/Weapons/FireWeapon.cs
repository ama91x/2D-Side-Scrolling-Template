using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireWeapon : MonoBehaviour
{
    private float _fireRateCoolDownTimer;

    private ActiveWeapon _activeWeapon;
    private FireWeaponEvent _fireWeaponEvent;
    private WeaponFiredEvent _weaponFiredEvent;

    private void Awake()
    {
        _activeWeapon = GetComponent<ActiveWeapon>();
        _fireWeaponEvent = GetComponent<FireWeaponEvent>();
        _weaponFiredEvent = GetComponent<WeaponFiredEvent>();
    }

    private void OnEnable()
    {
        _fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
    }

    private void OnDisable()
    {
        _fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
    }

    private void Update()
    {
        _fireRateCoolDownTimer -= Time.deltaTime;
    }

    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponFire(fireWeaponEventArgs);
    }

    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        if (fireWeaponEventArgs.Fire)
        {
            if (IsWeaponReadyToFire())
            {
                FireAmmo();

                ResetCoolDownTimer();
            }
        }
    }

    private bool IsWeaponReadyToFire()
    {
        if (_activeWeapon.GetCurrentWeapon().WeaponReaminingAmmo <= 0 && !_activeWeapon.GetCurrentWeapon().WeaponDetails.HasInfiniteAmmo)
        {
            return false;
        }

        if (_activeWeapon.GetCurrentWeapon().IsWeaponReloading)
        {
            return false;
        }

        if (_fireRateCoolDownTimer > 0f)
        {
            return false;
        }

        if (!_activeWeapon.GetCurrentWeapon().WeaponDetails.HasInfinitClipCapacity && _activeWeapon.GetCurrentWeapon().WeaponClipRemainingAmmo <= 0)
        {
            return false;
        }

        return true;
    }

    private void FireAmmo()
    {
        AmmoDetailsSO currentAmmo = _activeWeapon.GetCurrentAmmo();

        if (currentAmmo != null)
        {
            GameObject ammoPrefab = currentAmmo.AmmoPrefabArray[Random.Range(0, currentAmmo.AmmoPrefabArray.Length)];

            float ammoSpeed = Random.Range(currentAmmo.AmmoSpeedMin, currentAmmo.AmmoSpeedMax);

            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, _activeWeapon.GetShootPosition(), Quaternion.identity);

            ammo.InitialiseAmmo(currentAmmo, 0, ammoSpeed);

            if (!_activeWeapon.GetCurrentWeapon().WeaponDetails.HasInfinitClipCapacity)
            {
                _activeWeapon.GetCurrentWeapon().WeaponClipRemainingAmmo--;
                _activeWeapon.GetCurrentWeapon().WeaponReaminingAmmo--;
            }

            _weaponFiredEvent.CallWeaponFiredEvent(_activeWeapon.GetCurrentWeapon());
        }
    }

    private void ResetCoolDownTimer()
    {
        _fireRateCoolDownTimer = _activeWeapon.GetCurrentWeapon().WeaponDetails.WeaponFireRate;
    }
}
