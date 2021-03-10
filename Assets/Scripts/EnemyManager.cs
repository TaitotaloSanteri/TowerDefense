using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    private Vector3Int endPointCell;
    [SerializeField] private GameObject explosionPrefab;
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
    public static EnemyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        endPointCell = GridManager.instance.WorldToRoadCell(GameStateManager.Instance.levelData.endPoint.position);
    }

    public void TakeDamage(Enemy enemy, float damage, SpecialEffect effect)
    {
        if (!enemy) return; 
        enemy.health -= damage;

        Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);

        switch (effect)
        {
            case SpecialEffect.Freeze:
                enemy.moveSpeed *= 0.5f;
                break;
        }

        if (enemy.health <= 0)
        {
            GameStateManager.Instance.UpdateMoney(enemy.money);
            activeEnemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }

    private void Update()
    {
        if (GameStateManager.Instance.gameState == GameState.BUILDING)
        {
            return;
        }
        // Katsotaan onko mahdollista spawnata uusi vihollinen
        SpawnEnemies();

        if (GameStateManager.Instance.levelData.currentWave.maxEnemies == 0 &&
            activeEnemies.Count == 0)
        {
            GameStateManager.Instance.ChangeState(GameState.BUILDING);
        }

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

    private void SpawnEnemies()
    {
        // Jos maksimäärä vihollisia on spawnattu, niin ei spawnata niitä enempää
        if (GameStateManager.Instance.levelData.currentWave.maxEnemies <= 0)
        {
            return;
        }
        // Jos edellisestä spawnista on kulunut spawnIntervalin määrämä aika,
        // niin spawnataan uusi vihollinen.
        if (Time.time >= GameStateManager.Instance.levelData.spawnTime)
        {
            GameStateManager.Instance.levelData.spawnTime = Time.time + GameStateManager.Instance.levelData.currentWave.spawnInterval;
            GameStateManager.Instance.levelData.currentWave.maxEnemies--;

            // Etsitään kaikista vihollista ne viholliset, joiden level menee tämän hetkisen
            // waven minimi ja maksimi levelin väliin

            // Funktionaalinen ohjelmointi esimerkki (C# lambda expression)

            Enemy[] spawnableEnemies = Array.FindAll(enemies, enemyA => enemyA.level >= GameStateManager.Instance.levelData.currentWave.minEnemyLevel &&
            enemyA.level <= GameStateManager.Instance.levelData.currentWave.maxEnemyLevel);

            // Perinteinen for-loop tapa tehdä sama asia

            //List<Enemy> spawnableEnemies = new List<Enemy>();
            //for (int i = 0; i < enemies.Length; i++)
            //{
            //    if (enemies[i].level >= GameStateManager.Instance.levelData.currentWave.minEnemyLevel
            //        && enemies[i].level <= GameStateManager.Instance.levelData.currentWave.maxEnemyLevel)
            //    {
            //        spawnableEnemies.Add(enemies[i]);
            //    }
            //}


            int index = UnityEngine.Random.Range(0, spawnableEnemies.Length);
            Enemy enemy = Instantiate(spawnableEnemies[index], GameStateManager.Instance.levelData.spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
            SetDestination(enemy);
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
            GameStateManager.Instance.UpdateLives(-1);
        }

        // Päivitetään liikkumissuunta
        Direction dir = GridManager.instance.PathDirection(cell, enemy.from);
        // Jos vihollisen tämän hetkisen ruudun kohdalla on "ForceDirection" komponentti,
        // annetaan ForceDirectionin määrätä suunta. Tämä sitä varten, että vältetään
        // vihollisten tyhmä käyttäytyminen.
        foreach (ForceDirection fd in GameStateManager.Instance.levelData.forceDirections)
        {
            if (cell == fd.cellPosition)
            {
                int index = UnityEngine.Random.Range(0, fd.directions.Length);
                dir = fd.directions[index];
            }
        }
      
        // Päivitetään vihollisen tulosuunta seuraavaa liikkumissuuntaa varten
        enemy.from = ReverseDirection[dir];
        // Muutetaan tilemapista tulevat koordinaatit Unity -maailman koordinaateiksi
        enemy.destination = GridManager.instance.RoadCellToWorld(cell, dir);
    }
}
