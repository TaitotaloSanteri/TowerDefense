using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        Instantiate(enemies[0].transform, spawnPoint.position, Quaternion.identity);
    }
}
