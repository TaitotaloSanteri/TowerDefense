﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TowerButton towerButtonPrefab;
    private static ScriptableTower[] scriptableTowers;
    private static TowerButton[] towerButtons;
    [SerializeField] private Transform buttonStart;

    public static TowerButton currentlySelectedTowerButton;
    public static bool isPointerOnUI;

    private void OnValidate()
    {
        buttonStart = transform.Find("ButtonStart");
    }

    private void Awake()
    {
        // Etsitään kaikki ScriptableTower -tyyppiset assetit hakemistosta
        // Resources/Towers/
        scriptableTowers = Resources.LoadAll<ScriptableTower>("Towers");
        // Luodaan jokaista tornia varten oma nappulansa
        towerButtons = new TowerButton[scriptableTowers.Length];
        float xPosition = buttonStart.position.x;
        float yPosition = buttonStart.position.y;
        float spacing = 50f;
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i] = Instantiate(towerButtonPrefab, transform);
            towerButtons[i].towerName.text = scriptableTowers[i].towerName;
            towerButtons[i].towerCost.text = $"${scriptableTowers[i].towerCost}";
            towerButtons[i].transform.position = new Vector3(xPosition, yPosition - (i * spacing), 0f);
        }
    }

    private void Update()
    {
        isPointerOnUI = EventSystem.current.IsPointerOverGameObject();
    }

    private static ScriptableTower GetTower(TowerButton button)
    {
        for (int i = 0; i < scriptableTowers.Length; i++)
        {
            if (button == towerButtons[i])
            {
                return scriptableTowers[i];
            }
        }
        Debug.Log("Tower not found!");
        return null;
    }

    public static void SetCurrentlySelectedTowerButton(TowerButton button)
    {
        // Tarkistetaan, onko meillä jo nappula valittuna. Muutetaan nappulan
        // väri takaisin normaaliksi, jos on.
        if (currentlySelectedTowerButton != null)
        {
            currentlySelectedTowerButton.image.color = currentlySelectedTowerButton.colors.normalColor;
        }
        button.image.color = button.colors.selectedColor;
        currentlySelectedTowerButton = button;
        BuildingController.currentlySelectedTower = GetTower(button);
    }
  

}



