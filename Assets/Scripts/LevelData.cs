using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] public Transform spawnPoint, endPoint;
    [HideInInspector] public ForceDirection[] forceDirections;
    [HideInInspector] public float spawnTime;
    public float totalBuildingTime = 30f;
    public float spawnInterval;
    public int maxEnemies, maxEnemyLevel;

    private void Start()
    {
        forceDirections = GetComponentsInChildren<ForceDirection>();
        foreach(ForceDirection fd in forceDirections)
        {
            fd.cellPosition = GridManager.instance.WorldToRoadCell(fd.transform.position);
        }
    }

}

