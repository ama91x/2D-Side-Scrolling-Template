using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDetailsSO))]
public class WeaponDetailsSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("");

        WeaponDetailsSO weaponDetails = (WeaponDetailsSO)target;

        if (!weaponDetails.HasInfiniteAmmo)
        {
            weaponDetails.WeaponAmmoCapacity = EditorGUILayout.IntField("Weapon Ammo Capacity", weaponDetails.WeaponAmmoCapacity);
        }

        if (!weaponDetails.HasInfinitClipCapacity)
        {
            weaponDetails.WeaponClipAmmoCapacity = EditorGUILayout.IntField("Weapon Ammo Capacity", weaponDetails.WeaponClipAmmoCapacity);
        }

        if (weaponDetails.HasPrecharge)
        {
            weaponDetails.WeaponPrechargeTime = EditorGUILayout.FloatField("Weapon Precharge Time", weaponDetails.WeaponPrechargeTime);
        }

        EditorUtility.SetDirty(weaponDetails);
    }
}
