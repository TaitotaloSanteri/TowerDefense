using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Color valid, notValid;
    private SpriteRenderer indicatorSr;
    private void Awake()
    {
        indicator = Instantiate(indicator);
        indicatorSr = indicator.GetComponent<SpriteRenderer>();
    }

    public void HandleBuilding(Camera cam)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = GridManager.instance.WorldToBGCell(worldPos);
        indicator.transform.position = GridManager.instance.IndicatorPosition(cellPos);

        indicatorSr.color = UIManager.isPointerOnButton ? Color.clear : 
                            GridManager.instance.IsValidBuildingLocation(cellPos)
                            ? valid
                            : notValid;

        // On sama kuin: 

        //if (GridManager.instance.IsValidBuildingLocation(cellPos))
        //{
        //    indicatorSr.color = valid;
        //}
        //else
        //{
        //    indicatorSr.color = notValid;
        //}

    }
}
