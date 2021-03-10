using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Color valid, notValid;
    private SpriteRenderer indicatorSr;
    private Vector2 indicatorDefaultSize;
    private bool[,] hasBuilding;
    public static ScriptableTower currentlySelectedTower;

    private void Awake()
    {
        indicator = Instantiate(indicator);
        indicatorSr = indicator.GetComponent<SpriteRenderer>();
        indicatorDefaultSize = indicator.transform.localScale;
    }

    private void Start()
    {
        hasBuilding = new bool[GridManager.instance.mapSize.x, GridManager.instance.mapSize.y];
    }

    private Vector3Int Clamp(Vector3Int toClamp)
    {
        toClamp.x = Mathf.Clamp(toClamp.x, 0, GridManager.instance.mapSize.x - 1);
        toClamp.y = Mathf.Clamp(toClamp.y, 0, GridManager.instance.mapSize.y - 1);
        return toClamp;
    }

    public void HandleBuilding(Camera cam)
    {
        if (UIManager.isPointerOnUI || currentlySelectedTower == null)
        {
            indicatorSr.color = Color.clear;
            return;
        }

        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = GridManager.instance.WorldToBGCell(worldPos);

        indicator.transform.localScale = indicatorDefaultSize * currentlySelectedTower.size;
        indicator.transform.position = GridManager.instance.IndicatorPosition(cellPos);
        indicatorSr.color = valid;

        for (int x = 0; x < currentlySelectedTower.size.x; x++)
        {
            for (int y = 0; y < currentlySelectedTower.size.y; y++)
            {
                Vector3Int check = Clamp(new Vector3Int(cellPos.x + x, cellPos.y + y, 0));
                if (!GridManager.instance.IsValidBuildingLocation(check) ||
                    hasBuilding[check.x, check.y])
                {
                    indicatorSr.color = notValid;
                    return;
                }
            }
        }


        if (currentlySelectedTower.towerPrefab == null ||
            GameStateManager.Instance.playerData.money < currentlySelectedTower.towerCost)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            BaseTower tower = 
                Instantiate(currentlySelectedTower.towerPrefab, indicator.transform.position, Quaternion.identity);
            tower.towerStats = currentlySelectedTower;
            tower.SetStats();
            for (int x = 0; x < currentlySelectedTower.size.x; x++)
            {
                for (int y = 0; y < currentlySelectedTower.size.y; y++)
                {
                    Vector3Int check = Clamp(new Vector3Int(cellPos.x + x, cellPos.y + y, 0));
                    hasBuilding[check.x, check.y] = true;
                }
            }
            GameStateManager.Instance.UpdateMoney(-currentlySelectedTower.towerCost);
        }



    }
}
