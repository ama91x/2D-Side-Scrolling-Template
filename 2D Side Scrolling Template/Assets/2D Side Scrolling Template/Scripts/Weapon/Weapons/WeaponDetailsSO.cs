using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Details", menuName = "Scriptable Objects/Weapons/Weapon Details", order = 1)]
public class WeaponDetailsSO : ScriptableObject
{
    [Header("Weapons Base Details")]
    public string WeaponNames;
    public Sprite WeaponSprite;

    [Header("Weapon Configuration")]
    public Vector3 WeaponShootPosition;
    public AmmoDetailsSO WeaponCurrentAmmo;

    [Header("Weapon Operating Values")]
    public bool HasInfiniteAmmo = false;
    public bool HasInfinitClipCapacity = false;
    public bool HasPrecharge = false;

    [Tooltip("How many ammo the weapon can shots befor reload")]
    [HideInInspector] public int WeaponClipAmmoCapacity;
    [Tooltip("How many ammo can be held for this weapon (max ammo)")]
    [HideInInspector] public int WeaponAmmoCapacity;
    [Tooltip("Weapon fire rate - 0.2 means 5 shots a second")]
    public float WeaponFireRate;
    [Tooltip("Weapon precharge Time")]
    [HideInInspector] public float WeaponPrechargeTime;
    [Tooltip("Realod time in seconds")]
    public float WeaponReloadTime;
}
