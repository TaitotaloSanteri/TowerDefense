using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [HideInInspector] public GameState gameState;
    public PlayerData playerData;
    // Singleton
    public static GameStateManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // playerData = new PlayerData(1000);
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
    }
}

public enum GameState
{
    BUILDING,
    WAVE
}
