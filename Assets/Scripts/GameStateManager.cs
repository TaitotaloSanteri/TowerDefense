using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [HideInInspector] public GameState gameState;
    public PlayerData playerData;
    public LevelData levelData;
    // Singleton
    public static GameStateManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // playerData = new PlayerData(1000);
    }

    private void OnValidate()
    {
        levelData = FindObjectOfType<LevelData>();
    }

    private void Start()
    {
        UIManager.Instance.UpdateMoneyUI();
        ChangeState(GameState.BUILDING);
    }

    private void Update()
    {
        if (gameState == GameState.WAVE)
            return;
        levelData.currentWave.totalBuildingTime -= Time.deltaTime;
        if (levelData.currentWave.totalBuildingTime <= 0f)
        {
            ChangeState(GameState.WAVE);
        }
        UIManager.Instance.UpdateTimeText(levelData.currentWave.totalBuildingTime);
    }

    public void UpdateMoney(int cost)
    {
        playerData.money += cost;
        UIManager.Instance.UpdateMoneyUI();
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (gameState)
        {
            case GameState.WAVE:
                UIManager.Instance.timeText.enabled = false;
                break;
            case GameState.BUILDING:
                UIManager.Instance.timeText.enabled = true;
                levelData.ChangeWave();
                break;
        }
    }
}

public enum GameState
{
    BUILDING,
    WAVE
}
