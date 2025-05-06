using System;

public static class GameConfig
{
    public static Action OnGameOver;

    public const int ENEMY_COUNT_EACH_WAVE = 5;
    public const float EACH_ENEMY_SPAWN_DELAY = 1f;

    public const float MAX_SPAWN_xOFFSET = 5f;

    public const int ENEMY_GOLD_VALUE = 2;
    public const int BASE_WEAPON_GOLD_COST = 10;

    public const float FIRE_DELAY_THRESHOLD = 5f;
    public const float MAX_FIRE_RANGE = 7f;

    public const float POPUP_DURATION = 1f;
}
