using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    private LevelData levelData;
    private Vector3Int endPointCell;
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
    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Start()
    {
        levelData = FindObjectOfType<LevelData>();
        endPointCell = GridManager.instance.WorldToRoadCell(levelData.endPoint.position);
        activeEnemies.Add(Instantiate(enemies[0], levelData.spawnPoint.position, Quaternion.identity));
        SetDestination(activeEnemies[0]);
    }

    private void Update()
    {
        // Käydään kaikki viholliset läpi, jotka ovat vielä elossa
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            // Liikutetaan vihollista kohti sen määränpäätä
            activeEnemies[i].transform.position = 
                Vector3.MoveTowards(activeEnemies[i].transform.position,
                                    activeEnemies[i].destination,
                                    activeEnemies[i].moveSpeed * Time.deltaTime);
            // Jos määränpää on saavutettu, niin päivitetään uusi määränpää.
            if (activeEnemies[i].transform.position == activeEnemies[i].destination)
            {
                SetDestination(activeEnemies[i]);
            }
        }
    }

    private void SetDestination(Enemy enemy)
    {
        // Muutetaan vihollisen positio tilemap koordinaateiksi.
        Vector3Int cell = GridManager.instance.WorldToRoadCell(enemy.transform.position);

        if (cell == endPointCell)
        {
            activeEnemies.Remove(enemy);
            Destroy(enemy.gameObject);
            Debug.Log("Vihollinen pääsi loppuun asti");
        }
        // Päivitetään liikkumissuunta
        Direction dir = GridManager.instance.PathDirection(cell, enemy.from);
        // Päivitetään vihollisen tulosuunta seuraavaa liikkumissuuntaa varten
        enemy.from = ReverseDirection[dir];
        // Muutetaan tilemapista tulevat koordinaatit Unity -maailman koordinaateiksi
        enemy.destination = GridManager.instance.RoadCellToWorld(cell, dir);
    }
}
