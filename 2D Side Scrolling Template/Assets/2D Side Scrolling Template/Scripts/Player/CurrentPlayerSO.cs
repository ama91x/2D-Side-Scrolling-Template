// ----------------------------------------------------------------
//  Name:   Current Player SO
//  Des:    A calss inhereted from Scriptable Object class will 
//          store the selected player details
// ----------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "Current Player", menuName = "Scriptable Objects/Player/Current Player", order = 2)]
public class CurrentPlayerSO : ScriptableObject
{
    public PlayerDetailsSO PlayerDetails;

    public string PlayerName;
}
