using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TowerButton towerButtonPrefab;
    public ScriptableTower[] scriptableTowers;
    public TowerButton[] towerButtons;

    private void Awake()
    {
        // Etsitään kaikki ScriptableTower -tyyppiset assetit hakemistosta
        // Resources/Towers/
        scriptableTowers = Resources.LoadAll<ScriptableTower>("Towers");
        // Luodaan jokaista tornia varten oma nappulansa
        towerButtons = new TowerButton[scriptableTowers.Length];
        float xPosition = Screen.width * 0.8f;
        float yPosition = Screen.height * 0.8f;
        float spacing = 100f;
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i] = Instantiate(towerButtonPrefab, transform);
            towerButtons[i].towerName.text = scriptableTowers[i].towerName;
            towerButtons[i].towerCost.text = $"${scriptableTowers[i].towerCost}";
            towerButtons[i].GetComponent<RectTransform>().anchoredPosition = 
                                        new Vector3(xPosition, yPosition - (i * spacing), 0f);
        }
    }

}



