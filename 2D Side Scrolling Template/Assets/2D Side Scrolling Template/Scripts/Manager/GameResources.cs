using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources _instance;

    public static GameResources Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<GameResources>("Game Respurces");

            return _instance;
        }
    }

    [Space(10)]
    [Header("Player")]
    [Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
    [SerializeField] private CurrentPlayerSO _currentPlayer;

    public CurrentPlayerSO CurrentPlayer
    {
        get => _currentPlayer;
    }
}
