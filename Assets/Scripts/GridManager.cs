using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap bgMap, roadMap;    
    [HideInInspector] public Vector2Int mapSize;
    [HideInInspector] public float cellSize;

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
}
