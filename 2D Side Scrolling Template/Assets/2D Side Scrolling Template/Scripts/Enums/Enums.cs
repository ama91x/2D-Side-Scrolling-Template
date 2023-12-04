public enum GameStateTypes
{
    None,
    EndlessRunner,
    SidePlatformer
}

public enum GameStateEndlessRunner
{
    GameStarted,
    PlayingTheLevel,
    GameLost,
    GamePaused,
    RestartGame,
}

public enum GameStateSidePlatformer
{
    GameStarted,
    PlayingTheLevel,
    EngagingBoss,
    GameLost,
    GamePaused,
    RestartGame,
}

public enum SelectionType
{
    None,
    Player,
    Enemy
}