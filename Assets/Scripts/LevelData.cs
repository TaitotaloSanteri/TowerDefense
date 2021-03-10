using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] public Transform spawnPoint, endPoint;
    [HideInInspector] public ForceDirection[] forceDirections;
    [HideInInspector] public float spawnTime;
    [SerializeField] private Wave[] waves;
    private int waveIndex = 0;
    [HideInInspector] public Wave currentWave;

    private void Start()
    {
        forceDirections = GetComponentsInChildren<ForceDirection>();
        foreach(ForceDirection fd in forceDirections)
        {
            fd.cellPosition = GridManager.instance.WorldToRoadCell(fd.transform.position);
        }
    }

    public void ChangeWave()
    {
        currentWave = waves[waveIndex];
        waveIndex++;
        if (waveIndex >= waves.Length)
        {
            Debug.Log("Kaikki wavet käyty läpi");
            Application.Quit();
        }
    }

}

[System.Serializable]
public class Wave
{
    public float totalBuildingTime;
    public float spawnInterval;
    public int maxEnemies, minEnemyLevel, maxEnemyLevel;
}

