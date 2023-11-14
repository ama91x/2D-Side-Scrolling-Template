// ----------------------------------------------------------------
//  Name:   Current Player SO
//  Des:    A calss inhereted from Scriptable Object class will 
//          store the selected player details
// ----------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "Player Details", menuName = "Scriptable Objects/Player/Player Details", order = 1)]
public class CurrentPlayerSO : ScriptableObject
{
    public PlayerDetailsSO PlayerDetails;

    public string PlayerName;
}
