using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerButton : Button
{
    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerCost;

    public override void OnSelect(BaseEventData eventData){}
    public override void OnDeselect(BaseEventData eventData){}
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        UIManager.SetCurrentlySelectedTowerButton(this);
    }
}
