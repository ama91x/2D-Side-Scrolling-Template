using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Details", menuName = "Scriptable Objects/Movement/Movement Details", order = 1)]
public class MovementDetailsSO : ScriptableObject
{
    [HideInInspector] public SelectionType CharacterType;
    [HideInInspector] public float MovementSpeed = 8f;
    [HideInInspector] public float MinMovementSpeed = 8f;
    [HideInInspector] public float MaxMovementSpeed = 8f;

    public float GetMovementSpeed()
    {
        if (MinMovementSpeed <= 0)
            Debug.LogError("Min Move Speed Can't be 0 or less than 0");

        if (MinMovementSpeed > MaxMovementSpeed)
            Debug.LogError("Min Movement Speed can't be greter that Max Movement Speed");

        if (MinMovementSpeed == MaxMovementSpeed)
            return MinMovementSpeed;
        else
            return Random.Range(MinMovementSpeed, MaxMovementSpeed);
    }
}