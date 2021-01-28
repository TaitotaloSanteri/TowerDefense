using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Transform spawnPoint;
    // Käytetään tätä hakemaan menosuunnan vastainen suunta, jota
    // sitten käytetään vihollisen kohdalla tulosuuntana.
    private Dictionary<Direction, Direction> ReverseDirection
    = new Dictionary<Direction, Direction>()
    {
        {Direction.NORTH, Direction.SOUTH },
        {Direction.EAST, Direction.WEST },
        {Direction.SOUTH, Direction.NORTH},
        {Direction.WEST, Direction.EAST }
    };

    private Enemy activeEnemy;

    private void Start()
    {
        activeEnemy = enemies[0];
        activeEnemy.transform = Instantiate(enemies[0].transform, 
                                            spawnPoint.position, 
                                            Quaternion.identity);
    }
}
