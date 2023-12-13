using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Details", menuName = "Scriptable Objects/Weapons/Ammo Details", order = 2)]
public class AmmoDetailsSO : ScriptableObject
{
    [Header("Ammo Base Details")]
    public string AmmoName;
    public bool IsPlayerAmmo;

    [Header("Ammo Sprite - Prefab - Material")]
    public Sprite AmmoSprite;
    public GameObject[] AmmoOrefabArray;
    public Material AmmoMaterial;
    public float AmmoChargeTime;
    public Material AmmoChargeMaterial;

    [Header("Ammo Operating Values")]
    public int AmmoDamage;
    public float AmmoSpeedMin;
    public float AmmoSpeedMax;
    public float AmmoRange;
    public float AmmoRotationSpeed;

    [Header("Ammo Spread Details")]
    public float AmmoSpreadMin;
    public float AmmoSpreadMax;

    [Header("Ammo Spawn Details")]
    public int AmmoSpawnAmountMin;
    public int AmmoSpawnAmountMax;
    public float AmmoSpawnIntervalMin;
    public float AmmoSpawnIntervalMax;

    [Header("Ammo Trail Details")]
    public bool IsAmmoTrail = false;
    public float AmmoTrailTime;
    public Material AmmoTrailsMaterial;
    public float AmmoTrailsStartWidth;
    public float AmmoTrailsEndWidth;
}
