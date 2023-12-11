// ----------------------------------------------------------------
//  Name:   Player Details SO
//  Des:    A calss inhereted from Scriptable Object class containn 
//          all the details for a spasific player
// ----------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "Player Details", menuName = "Scriptable Objects/Player/Player Details", order = 1)]
public class PlayerDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("Player Base Details")]
    [Tooltip("Player character name.")]
    public string PlayerCharacterName;

    [Tooltip("Player prefab game object")]
    public GameObject PlayerPrefab;

    [Tooltip("Player run time animator controller")]
    public RuntimeAnimatorController PlayerRuntimeAnimatorController;



    [Space(10)]
    [Header("Health")]
    [Tooltip("Player starting health")]
    public int PlayerHealthAmount;

    [Space(10)]
    [Header("Others")]
    [Tooltip("Player hand sprite")]
    public Sprite PlayerHandSprite;
}
