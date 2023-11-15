using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    // Inspector Assigne
    [Header("Select Game State Type")]
    [SerializeField] private GameStateTypes _gameStateType;

    [Header("Spown Position")]
    [SerializeField] private Transform _playerSpownPosition;

    // Private
    private PlayerDetailsSO _playerDetails;
    private Player _player;

    private GameStateEndlessRunner _gameStateEndlessRunner;
    private GameStateSidePlatformer _gameStateSidePlatformer;

    // Accessors & Mutators
    public GameStateEndlessRunner GameStateEndlessRunners
    {
        get => _gameStateEndlessRunner;
    }

    public GameStateSidePlatformer GameStateSidePlatformer
    {
        get => _gameStateSidePlatformer;
    }

    protected override void Awake()
    {
        base.Awake();

        // Set Player Details - saved in current player scriptable object from the main menu
        _playerDetails = GameResources.Instance.CurrentPlayer.PlayerDetails;

        InstantiatePlayer();
    }

    private void Start()
    {
        if (_gameStateType == GameStateTypes.None)
        {
            Debug.Log("You did not select and game state type");
            return;
        }

        if (_gameStateType == GameStateTypes.EndlessRunner)
        {
            _gameStateEndlessRunner = GameStateEndlessRunner.GameStarted;
        }

        if (_gameStateType == GameStateTypes.SidePlatformer)
        {
            _gameStateSidePlatformer = GameStateSidePlatformer.GameStarted;
        }
    }

    private void Update()
    {
        if (_gameStateType == GameStateTypes.None)
            return;

        if (_gameStateType == GameStateTypes.EndlessRunner)
            HandleGameStateEndlessRunner();

        if (_gameStateType == GameStateTypes.SidePlatformer)
            HandleGameStateSidePlatformer();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_gameStateType == GameStateTypes.EndlessRunner)
                _gameStateEndlessRunner = GameStateEndlessRunner.GameStarted;

            if (_gameStateType == GameStateTypes.SidePlatformer)
                _gameStateSidePlatformer = GameStateSidePlatformer.GameStarted;
        }
    }

    private void InstantiatePlayer()
    {
        GameObject playerGameObject = Instantiate(_playerDetails.PlayerPrefab);

        _player = playerGameObject.GetComponent<Player>();

        _player.Initialize(_playerDetails);
    }

    private void HandleGameStateEndlessRunner()
    {
        switch (_gameStateEndlessRunner)
        {
            case GameStateEndlessRunner.GameStarted:
                PlayMapLevelGameStateEndlessRunner();
                _gameStateEndlessRunner = GameStateEndlessRunner.PlayingTheLevel;
                break;
        }
    }

    private void HandleGameStateSidePlatformer()
    {
        switch (_gameStateSidePlatformer)
        {
            case GameStateSidePlatformer.GameStarted:
                PlayMapLevelGameStateSidePlatformer();
                _gameStateSidePlatformer = GameStateSidePlatformer.PlayingTheLevel;
                break;
        }
    }

    private void PlayMapLevelGameStateEndlessRunner()
    {
        // Spowning The player
        _player.gameObject.transform.position = _playerSpownPosition.position;

        // TODO: Instantiate The Map and Obsticole
    }

    private void PlayMapLevelGameStateSidePlatformer()
    {
        _player.gameObject.transform.position = _playerSpownPosition.position;
    }

    public Player GetPlayer()
    {
        return _player;
    }
}
