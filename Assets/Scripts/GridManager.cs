using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap bgMap, roadMap;    
    [HideInInspector] public Vector2Int mapSize;
    [HideInInspector] public float cellSize;

    private Dictionary<Direction, Vector3Int> DirToVector3
    = new Dictionary<Direction, Vector3Int>()
    {
        {Direction.NORTH, new Vector3Int(0, 1, 0)},
        {Direction.EAST, new Vector3Int(1, 0, 0)},
        {Direction.SOUTH, new Vector3Int(0, -1, 0)},
        {Direction.WEST, new Vector3Int(-1, 0, 0)}
    };
    
// Tehdään Singleton -muuttuja
public static GridManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Haetaan kartan koko Background Tilemapin mukaan
        mapSize = (Vector2Int) bgMap.size;
        // Haetaan yksittäisen Tilen koko Grid -komponentista löytyvästä cellsize
        // muuttujasta
        cellSize = GetComponent<Grid>().cellSize.x;
    }

    public Vector3Int WorldToBGCell(Vector3 worldPos)
    {
        return bgMap.WorldToCell(worldPos);
    }

    public Vector3Int WorldToRoadCell(Vector3 worldPos)
    {
        return roadMap.WorldToCell(worldPos);
    }

    public Vector3 RoadCellToWorld(Vector3Int cellPos, Direction to)
    {
        return roadMap.GetCellCenterWorld(cellPos + DirToVector3[to]);
    }

    public Direction PathDirection(Vector3Int currentCell, Direction from)
    {
        // Luodaan lista kaikkien mahdollisten suuntien varalle.
        List<Direction> possiblePaths = new List<Direction>();
        // Käydään for loopilla jokainen ilmansuunta läpi
        for (int i = 0; i < 4; i++)
        {
            Direction dir = (Direction)i;
            // Jos suunta on sama, kuin saapumissuunta, ei lisätä sitä mahdollisten
            // suuntien listalle.
            if (dir == from)
            {
                continue;
            }

            if (roadMap.HasTile(currentCell + DirToVector3[dir]))
            {
                possiblePaths.Add(dir);
            }
        }

        if (possiblePaths.Count == 0)
        {
            return from;
        }
        else
        {
            int index = UnityEngine.Random.Range(0, possiblePaths.Count);
            return possiblePaths[index];
        }
    }
    

}
