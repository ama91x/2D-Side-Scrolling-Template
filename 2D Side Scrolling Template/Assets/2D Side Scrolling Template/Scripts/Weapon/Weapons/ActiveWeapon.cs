using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField] private Transform _weaponShootPosition;
    [SerializeField] private Transform _weaponEffectPosition;

    private SetActiveWeaponEvent _setWeaponEvent;
    private Weapon _currentWeapon;

    private void Awake()
    {
        _setWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        _setWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;
    }

    private void OnDisable()
    {
        _setWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;
    }

    private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {
        SetWeapon(setActiveWeaponEventArgs.Weapon);
    }

    private void SetWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;

        _weaponSpriteRenderer.sprite = _currentWeapon.WeaponDetails.WeaponSprite;

        _weaponShootPosition.localPosition = _currentWeapon.WeaponDetails.WeaponShootPosition;
    }

    public AmmoDetailsSO GetCurrentAmmo()
    {
        return _currentWeapon.WeaponDetails.WeaponCurrentAmmo;
    }

    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public Vector3 GetShootPosition()
    {
        return _weaponShootPosition.position;
    }

    public Vector3 GetShootEffectPosition()
    {
        return _weaponEffectPosition.position;
    }

    public void RemoveCurrentWeapon()
    {
        _currentWeapon = null;
    }
}
